using pco;
using System;
using System.Threading;

namespace SimpleExample
{
  class SimpleExample
  {
    static void Main(string[] args)
    {
      uint err = 0;
      string filename;
      string filenameRaw = "example_raw.tif";
      string saveType;
      string saveTypeRaw = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BW_16;
      bool isBitmap;
      Image img = new Image();
      int image_count = 10;
      Camera cam = new Camera();
      Console.WriteLine("Enter Filepath where Images should go (default: .):");
      string path = Console.ReadLine();
      if (path == null || path.Length == 0)
        path = "";
      cam.defaultConfiguration();
      cam.setExposureTime(0.01);

      if (cam.isColored())
      {
        filename = "example.bmp";
        isBitmap = true;
        saveType = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BGR_8;
      }
      else
      {
        filename = "example.tif";
        isBitmap = false;
        saveType = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BW_8;
      }


      cam.record(image_count);
      cam.waitForFirstImage();
      for (uint counter = 0; counter < image_count; counter++)
      {
        string name = path + counter + "_" + filename;
        string nameRaw = path + counter + "_" + filenameRaw;
        Console.WriteLine("Image count: " + (counter+1) + " > " + name);
        Console.WriteLine("Raw Image: " + (counter + 1) + " > " + nameRaw);
        if (cam.isColored())
        {
          cam.image(img, counter, null, DataFormat.BGR8);
          err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img.vector_8bit(), (UInt16)img.width(), (UInt16)img.height(), saveType, isBitmap, name, true, IntPtr.Zero);
        }
        else
        {
          cam.image(img, counter, null, DataFormat.Mono8);
          err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img.vector_8bit(), (UInt16)img.width(), (UInt16)img.height(), saveType, isBitmap, name, true, IntPtr.Zero);
        }
        if (err != 0)
          throw new Camera_Exception(err);

        // Save also raw image
        err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img.raw_vector_16bit(), (UInt16)img.width(), (UInt16)img.height(), saveTypeRaw, false, nameRaw, true, IntPtr.Zero);

        if (err != 0)
          throw new Camera_Exception(err);
      }
      //We suggest to actively close the cameras to avoid garbage collection errors
      cam.close();
    }
  }
}
