using BCMT_Associates.Context;
using BCMT_Associates.Models;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.ComponentModel;
using System.Drawing;


namespace BCMT_Associates.Services
{
    public class UploadFileService
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        public UploadFileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<Array[]> UploadedFile(IFormFileCollection files, string folder)
        {

            string uniqueFileName = String.Empty;
            Array[] arr = null;
            byte[] array = null;
            int i = 0;
            if (files.Count > 0)
            {

                arr = new Array[files.Count];

                Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath, folder));
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                foreach (var file in files)
                {
                    if (!Common.FileExtention(file.FileName))
                    {
                        return arr;
                    }
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var inputStream = new FileStream(filePath, FileMode.Create))
                    {
                        // read file to stream
                        await file.CopyToAsync(inputStream);
                        // stream to byte array
                        array = new byte[inputStream.Length];
                        inputStream.Seek(0, SeekOrigin.Begin);
                        inputStream.Read(array, 0, array.Length);
                        arr[i] = array;
                        i++;
                    }
                }

            }

            return arr;
        }

		public async Task<byte[][]> UploadAndCompressImages(IFormFileCollection files, string folder, int quality)
		{
			if (files == null || files.Count == 0)
			{
				return null; // No files to upload and compress
			}

			// Create an array to store compressed image bytes for each uploaded file
			byte[][] compressedImages = new byte[files.Count][];

			// Create the folder if it doesn't exist
			Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath, folder));
			string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);

			for (int i = 0; i < files.Count; i++)
			{
				var file = files[i];
				string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);

				if (!Common.FileExtention(file.FileName))
				{
					return null; // Invalid file extension
				}

				// Upload the file
				using (var inputStream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(inputStream);
				}

				// Add a delay to ensure the file is closed before attempting to compress it
				await Task.Delay(1000); // Adjust the delay time as needed

				// Load the uploaded image and compress it
				byte[] compressedImage = await CompressImage(filePath, quality);
				compressedImages[i] = compressedImage;
			}

			return compressedImages;
		}


		public async Task<byte[]> CompressImage(string imagePath, int quality)
		{
			// Load the image using SixLabors.ImageSharp
			using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(imagePath))
			{
				// Compress the image
				using (var output = new MemoryStream())
				{
					await Task.Run(() =>
					{
						image.Save(output, new JpegEncoder()
						{
							Quality = quality
						});
					});

					// Return the compressed image bytes
					return output.ToArray();
				}
			}
		}



		public async Task<string> UploadExcelFile(IFormFileCollection files, string folder)
        {

            string uniqueFileName = String.Empty;
            string filePath = "";
            Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath, folder));
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
            //var customerList = new List<Customer>();
            if (files.Count > 0)
            {

                foreach (var file in files)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                        return ("Invalid file format");


                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var inputStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(inputStream);
                    }
                }
            }
            return filePath;

        }


    }
}
