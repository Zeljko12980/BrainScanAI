namespace Patient.Domain.Entities;

public class ScanImage(Guid patientId, string imageType, string url, DateTime takenAt)
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string ImageType { get; private set; } =
        !string.IsNullOrWhiteSpace(imageType) ? imageType : throw new ArgumentException("Image type is required.", nameof(imageType));

    public string Url { get; private set; } =
        !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentException("Image URL is required.", nameof(url));

    public DateTime TakenAt { get; private set; } = takenAt;

    public Guid PatientId { get; private set; } = patientId;

    public Patient Patient { get; private set; } = null!;

    public void UpdateUrl(string newUrl)
    {
        if (string.IsNullOrWhiteSpace(newUrl))
            throw new ArgumentException("New image URL is required.", nameof(newUrl));

        Url = newUrl;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ScanImage other) return false;
        return Id == other.Id;
    }

    public static ScanImage Create(Guid patientId, string imageType, string url, DateTime takenAt)
    {
        return new ScanImage(patientId, imageType, url, takenAt);
    }


    public override int GetHashCode() => Id.GetHashCode();
}
