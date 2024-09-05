using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using OwlReadingRoom.Services;
using OwlReadingRoom.Utils;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace OwlReadingRoom.Platforms.Windows.Services
{
    public class PdfService : IPdfService
    {
        public async Task<MemoryStream> CaptureViewAsync(View view)
        {
            var nativeView = view.Handler.PlatformView as FrameworkElement;
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(nativeView);

            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();

            using var stream = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetPixelData(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Premultiplied,
                (uint)renderTargetBitmap.PixelWidth,
                (uint)renderTargetBitmap.PixelHeight,
                96,
                96,
                pixels);

            await encoder.FlushAsync();

            var memoryStream = new MemoryStream();
            var randomAccessStream = stream.AsStreamForRead();
            await randomAccessStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task DownloadAsync(View view)
        {
            try
            {
                // Capture only the provided view as an image
                MemoryStream imageStream;
                imageStream = await CaptureViewAsync(view);

                if (imageStream == null || imageStream.Length == 0)
                {
                    throw new InvalidOperationException("Failed to capture the receipt content.");
                }

                // Create a PDF document using iText
                using (var memoryStream = new MemoryStream())
                {
                    var writerProperties = new WriterProperties().SetCompressionLevel(CompressionConstants.DEFAULT_COMPRESSION);
                    using (var writer = new PdfWriter(memoryStream, writerProperties))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);

                            // Add the image to the PDF
                            var pdfImg = ImageDataFactory.Create(imageStream.ToArray());
                            document.Add(new iText.Layout.Element.Image(pdfImg).SetAutoScale(true));

                            // Close the document
                            document.Close();
                        }
                    }

                    // Get the PDF bytes
                    byte[] pdfBytes = memoryStream.ToArray();

                    // Save the PDF file
                    var filePath = Path.Combine(FileSystem.AppDataDirectory, "Receipt.pdf");
                    await File.WriteAllBytesAsync(filePath, pdfBytes);

                    // Open the saved PDF file
                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(filePath)
                    });
                }
            }
            catch (PlatformNotSupportedException ex)
            {
                await CustomAlert.ShowAlert("Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await CustomAlert.ShowAlert("Error", $"An error occurred while creating the PDF: {ex.Message}", "OK");
            }
        }
    }
}
