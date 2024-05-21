export interface IHomePageStats {
  Partners: number;
  Clients: number;
  BuildingStats: {
    NumberOfBuildings: number;
    TotalGLA: number;
    TotalNumberOfMeters: number;
  };
  TenantStats: {
    NumberOfTenants: number;
    OccupiedPercentage: number;
    RecoverablePercentage: number;
  };
  Tenants: {
    NumberOfTenants: number;
    OccupiedPercentage: number;
    RecoverablePercentage: number;
  };
  TenantSlips: {
    Tenants: number;
    Shops: number;
    Amount: number;
  };
  ShopStats: {
    NumberOfShops: number;
    OccupiedPercentage: number;
    TotalArea: number;
  };
  SmartStats: {
    BulkCount: number;
    ConsumerElectricityCount: number;
    ConsumerWaterCount: number;
    CouncilChkCount: number;
    GeneratorCount: number;
    SolarCount: number;
    TotalSmart: number;
    AlarmsConfigured: number;
    AlarmsTriggered: number;
  };
  SmartServices: {
    TotalSmart: number;
    Electricity: number;
    Water: number;
  };
  GraphStats: any[];
  Stats: any[];
  Users: number
  Shops:{
    NumberOfShops: number;
    OccupiedPercentage: number;
    TotalArea: number;
  };
}


  export enum EHomeTabType {
    Buildings,
    Tenants,
    Shops,
    SmartServices
  }

  export const CHomeTabTypeText = {
    [EHomeTabType.Buildings]: 'Buildings',
    [EHomeTabType.Tenants]: 'Tenants',
    [EHomeTabType.Shops]: 'Shops',
  };
  export interface IHomeTab {
    id: number;
    title: string;
    type?: string;
    apiEndpoint?: string;
    dataSource?: any[] | any;
    detail?: any;
  }