using pco;
using pco.sdk;
using System;
using System.Threading;
using System.Linq;

namespace SimpleExample_Softtrigger
{
  class SimpleExample_Softtrigger
  {
    static void Main(string[] args)
    {
      Image img = new Image();
      Camera cam = new Camera();

      ushort triggered = 0; 
      int image_count = 10;
      double cur_exposure = 0.01;

      Configuration config = cam.getConfiguration();
      config.trigger_mode = PCO_SDK_DEFINES.TRIGGER_MODE_SOFTWARETRIGGER;
      cam.setConfiguration(config);

      cam.record(10, RecordMode.sequence_non_blocking);

      for (uint counter = 0; counter < image_count; counter++)
      {
        cam.setExposureTime(cur_exposure);
        PCO_SDK.PCO_ForceTrigger(cam.sdk, ref triggered);
        cam.waitForNewImage();

        cam.image(img, counter, data_format: DataFormat.Mono16);

        //We compute the mean hear just to have something to show
        double mean = img.vector_16bit().Select(x => (int)x).Average();
        Console.WriteLine("Image " + counter + " fetched with mean " + mean);
        cur_exposure += 0.01;
      }

      //Now you have 10 images with exposure times from 0.01 to 0.1
      //sequence mode should automatically stop, but just to be safe we recommend to put a stop here, anyway
      cam.stop();

      cam.close();
    }
  }
}
