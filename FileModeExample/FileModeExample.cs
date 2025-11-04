using pco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileModeExample
{
  class FileModeExample
  {
    static void Main(string[] args)
    {
      Image img = new Image();
      int image_count = 10;
      Camera cam = new Camera();

      Console.WriteLine("Enter Filepath where Images should go (default: .):");
      string path = Console.ReadLine();
      if (path == null || path.Length == 0)
        path = "./";

      cam.defaultConfiguration();
      cam.setExposureTime(0.01);
      uint counter = 0;
      cam.record(image_count, RecordMode.tif, path);
      while (cam.isRecording())
      {
        if (counter != cam.getRecordedImageCount())
          counter = cam.getRecordedImageCount();
        else
        {
          Thread.Sleep(1);
          continue;
        }
        Console.WriteLine("Image Count " + counter + " > " + path);
      }
      //We suggest to actively close the cameras to avoid garbage collection errors
      cam.close();
    }
  }
}
