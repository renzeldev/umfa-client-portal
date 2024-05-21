import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { formatDateString, formatTimeString } from '@core/utils/umfa.help';
import { UmfaUtils } from '@core/utils/umfa.utils';
import { AlarmConfigurationService } from '@shared/services/alarm-configuration.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-alarm-night-flow',
  templateUrl: './alarm-night-flow.component.html',
  styleUrls: ['./alarm-night-flow.component.scss']
})
export class AlarmNightFlowComponent implements OnInit {
  @Input() meter;
  @Output() onChangeGraph: EventEmitter<any> = new EventEmitter<any>();
  @Output() save: EventEmitter<any> = new EventEmitter<any>();
  @Output() delete: EventEmitter<any> = new EventEmitter<any>();

  minutues = [];
  form: FormGroup;
  analyzeForm: FormGroup;
  configInfo: any;
  analyzeInfo: any;
  alarmMeterDetail: any;
  active: boolean = true;
  private _unsubscribeAll: Subject<any> = new Subject<any>();

  constructor(
    private _alarmConfigService: AlarmConfigurationService,
    private _formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    for(let i = 1; i <= 10; i++) {
      this.minutues.push({Value: i * 30});
    }

    // form 
    this.form = this._formBuilder.group({
      NightStartTime: [],
      NightEndTime: []
    })

    this.analyzeForm = this._formBuilder.group({
      Duration: ['', [Validators.required]],
      Threshold: ['', [Validators.required]]
    });

    this._alarmConfigService.alarmMeterDetail$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((data: any) => {
        this.alarmMeterDetail = data.find(obj => obj.AlarmTypeId == 1);
        if(this.alarmMeterDetail) {
          let startDate = new Date();
          let endDate = new Date();
          startDate.setHours(Number(this.alarmMeterDetail['StartTime'].split(':')[0]))
          startDate.setMinutes(Number(this.alarmMeterDetail['StartTime'].split(':')[1]));

          endDate.setHours(Number(this.alarmMeterDetail['EndTime'].split(':')[0]))
          endDate.setMinutes(Number(this.alarmMeterDetail['EndTime'].split(':')[1]));

          this.form.patchValue({
            NightStartTime:  startDate,
            NightEndTime: endDate
          });

          this.analyzeForm.patchValue({
            Duration: this.alarmMeterDetail['Duration'],
            Threshold: this.alarmMeterDetail['Threshold'],
          });

          this.active = this.alarmMeterDetail['Active'];
        }
      });
  }

  onAlarmConfigNightFlow() {
    let formData = this.form.value;
    if(this._alarmConfigService.profileInfo) {
      let nStartTime = {hours: formData['NightStartTime'].getHours(), minutes: formData['NightStartTime'].getMinutes()};
      let nEndTime = {hours: formData['NightEndTime'].getHours(), minutes: formData['NightEndTime'].getMinutes()};

      this._alarmConfigService.profileInfo = {...this._alarmConfigService.profileInfo, NightFlowStart: nStartTime, NightFlowEnd: nEndTime}
      let data = {  
        ProfileStartDTM: formatDateString(this._alarmConfigService.profileInfo.StartDate), 
        ProfileEndDTM: formatDateString(this._alarmConfigService.profileInfo.EndDate), 
        NFStartTime: formatTimeString(nStartTime), 
        NFEndTime: formatTimeString(nEndTime),
        MeterSerialNo: this._alarmConfigService.profileInfo.MeterSerialNo
      };
      this._alarmConfigService.getAlarmConfigNightFlow(data).subscribe(res => {
        if(res && res['MeterConfig']) this.configInfo = res['MeterConfig'];
        if(res && res['MeterData']) this.onChangeGraph.emit(res['MeterData']);
      });
    } else {
      this._alarmConfigService.showAlert('You should set profile option first!');
    }
  }

  getAlarmAnalyzeNightFlow() {
    if(this.analyzeForm.valid) {
      if(this._alarmConfigService.profileInfo) {
        let configData = this.form.value;
        let nStartTime = {hours: configData['NightStartTime'].getHours(), minutes: configData['NightStartTime'].getMinutes()};
        let nEndTime = {hours: configData['NightEndTime'].getHours(), minutes: configData['NightEndTime'].getMinutes()};

        let data = {  
          ...this.analyzeForm.value,
          ProfileStartDTM: formatDateString(this._alarmConfigService.profileInfo.StartDate), 
          ProfileEndDTM: formatDateString(this._alarmConfigService.profileInfo.EndDate), 
          NFStartTime: formatTimeString(nStartTime), 
          NFEndTime: formatTimeString(nEndTime),
          MeterSerialNo: this._alarmConfigService.profileInfo.MeterSerialNo,
        };
        this._alarmConfigService.getAlarmAnalyzeNightFlow(data).subscribe(res => {
          if(res && res['Alarms']) this.analyzeInfo = res['Alarms'];
          if(res && res['MeterData']) this.onChangeGraph.emit(res['MeterData']);
        });
      } else {
        this._alarmConfigService.showAlert('You should set profile option first!');
      }
    }
  }

  onSave() {
    let configData = this.form.value;
    let nStartTime = {hours: configData['NightStartTime'].getHours(), minutes: configData['NightStartTime'].getMinutes()};
    let nEndTime = {hours: configData['NightEndTime'].getHours(), minutes: configData['NightEndTime'].getMinutes()};

    let data = {
      ...this.analyzeForm.value,
      AMRMeterAlarmId: this.alarmMeterDetail ? this.alarmMeterDetail.AMRMeterAlarmId : 0,
      StartTime: formatTimeString(nStartTime),
      EndTime: formatTimeString(nEndTime),
      Active: this.active
    };
    console.log('before data', data);
    this.save.emit(data);
  }

  onRemove() {
    this.delete.emit(true);
  }

  ngOnDestroy(): void {
    this._alarmConfigService.destroy();
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }
}
