using ClientPortal.Models.RequestModels;

namespace ClientPortal.Data.Entities.PortalEntities
{
    [Serializable]
    public class ScadaRequestHeader
    {
        public int Id { get; set; }
        /// <summary>
        /// 0: not busy
        /// 1: scheduled to run
        /// 2: running
        /// 3: successfully retrieved Data from Scada
        /// 4: Inserted in DB
        /// 5: Processed profile data
        /// 6: error
        /// </summary>
        public int Status { get; set; } //0: not busy, 1: scheduled to run, 2: running, 3: successfully retrieved, 4: Inserted in DB , 5: Processed profile data, 6: error
        public bool Active { get; set; }
        public DateTime CreatedDTM { get; set; }
        public DateTime StartRunDTM { get; set; }
        public DateTime? LastRunDTM { get; set; }
        public DateTime? CurrentRunDTM { get; set; }
        public List<ScadaRequestDetail> ScadaRequestDetails { get; set; }
        /// <summary>
        /// 1: Retreive Profile Data
        /// 2: Retreive Reading Data
        /// </summary>
        public int JobType { get; set; } //1 for profile, 2 for readings
        public string Description { get; set; }
        /// <summary>
        /// The task to check for new jobs to run executes every 10 minutes.
        /// If larger offset is required, use this setting to set the time to wait after last execution.
        /// In minutes
        /// </summary>
        public int Interval { get; set; } //time to execute after previous run in minutes

        public void Map(ScadaRequestHeaderRequest request)
        {
                Status = request.Status;
                Active = request.Active;
                CreatedDTM = request.CreatedDTM;
                StartRunDTM = request.StartRunDTM;
                LastRunDTM = request.LastRunDTM;
                CurrentRunDTM = request.CurrentRunDTM;
                JobType = request.JobType;
                Description = request.Description;
                Interval = request.Interval;
        }
    }
}
