namespace OwlReadingRoom.Services
{
    public interface IPdfService
    {
        /// <summary>
        /// Downloads the selected view for download.
        /// The view is captured as a screenshot/image and then converted to form an pdf using iText7 library
        /// </summary>
        /// <param name="view">The selected view to be processed for pdf download.</param>
        /// <returns>Downloaded Pdf file.</returns>
        Task DownloadAsync(View view);

        /// <summary>
        /// Captures the provided view as a screenshot using bitmap encoder and memory streams.
        /// </summary>
        /// <param name="view">The selected view to be processed for pdf download.</param>
        /// <returns>The memory stream of captured image of the selected view.</returns>
        Task<MemoryStream> CaptureViewAsync(View view);
    }
}
