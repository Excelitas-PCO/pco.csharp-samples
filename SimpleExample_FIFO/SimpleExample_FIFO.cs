using pco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExample_FIFO
{
  class SimpleExample_FIFO
  {
    static void Main(string[] args)
    {
      uint err = 0;
      Image img = new Image();
      int image_count = 10;
      Camera cam = new Camera();
      Console.WriteLine("Enter Filepath where Images should go (default: .):");
      string path = Console.ReadLine();
      if (path == null || path.Length == 0)
        path = "";
      cam.defaultConfiguration();
      cam.setExposureTime(0.01);

      cam.record(image_count, RecordMode.fifo);

      for (uint counter = 0; counter < image_count; counter++)
      {
        string name = path + counter + "_" + "FIFO_img_" + (counter + 1) + ".tif";
        string nameRaw = path + counter + "_" + "FIFO_img_" + (counter + 1) + "_raw.tif";
        cam.waitForNewImage();

        if (cam.isColored())
        {
          cam.image(img, 0, null, DataFormat.BGR8);
          err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img.vector_8bit(), (UInt16)img.width(), (UInt16)img.height(), pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BGR_8, true, name, true, IntPtr.Zero);
        }
        else
        {
          cam.image(img, 0, null, DataFormat.Mono8);
          err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img.vector_8bit(), (UInt16)img.width(), (UInt16)img.height(), pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BW_8, true, name, true, IntPtr.Zero);
        }
        Console.WriteLine("Image Count: " + counter + " > " + name);
        err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img.raw_vector_16bit(), (UInt16)img.width(), (UInt16)img.height(), pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BW_16, true, nameRaw, true, IntPtr.Zero);
        Console.WriteLine("Raw Image Count: " + counter + " > " + nameRaw);
      }
      //We suggest to actively close the cameras to avoid garbage collection errors
      cam.close();
    }
  }
}
