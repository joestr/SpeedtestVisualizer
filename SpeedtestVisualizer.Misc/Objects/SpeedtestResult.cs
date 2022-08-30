using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeedtestVisualizer.Misc.Objects;

public class SpeedtestResult
{
    public SpeedtestResult(DateTime measuringTimestamp, Int64 downstreamBps, Int64 upstreamBps)
    {
        MeasuringTimestamp = measuringTimestamp;
        DownstreamBps = downstreamBps;
        UpstreamBps = upstreamBps;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 Id { get; set; }
    public DateTime MeasuringTimestamp { get; set; }
    public Int64 DownstreamBps { get; set; }
    public Int64 UpstreamBps { get; set; }
}