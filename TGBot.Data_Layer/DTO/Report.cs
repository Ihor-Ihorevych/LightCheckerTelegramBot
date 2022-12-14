using System.Text.Json.Serialization;

namespace TGBot.Data.DTO;

/// <summary>
///     Ths object represents a report about light status.
/// </summary>
public class Report
{
    /// <summary>
    ///     Time difference between the last report and the current time.
    /// </summary>
    [JsonPropertyName("TimeDiff")]
    public int TimeDifference { get; set; }

    /// <summary>
    ///     The current light status.
    /// </summary>
    public string PowerStatus { get; set; }

    /// <summary>
    ///     Time of the last report.
    /// </summary>
    [JsonPropertyName("LastObserverdTime")]
    public DateTimeOffset LastObservedTime { get; set; }

    /// <summary>
    ///     The current time.
    /// </summary>
    public DateTimeOffset CurrentTime { get; set; }


    public override string ToString()
    {
        return $"Time difference: {TimeDifference} seconds, Power status: {PowerStatus}, Last observed time: {LastObservedTime}, Current time: {CurrentTime}";
    }
}




