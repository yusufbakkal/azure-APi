namespace deneme.Models
{
    
        internal class AzureTrafficResponse
        {
            public FlowSegmentData flowSegmentData { get; set; }

            public class FlowSegmentData
            {
                public string Frc { get; set; }
                public double currentSpeed { get; set; }
                public double freeFlowSpeed { get; set; }
                public int currentTravelTime { get; set; }
                public int freeFlowTravelTime { get; set; }
                public double confidence { get; set; }
            }
        }

    }

