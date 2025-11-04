using pco;
using System;
using System.Collections;

namespace SimpleExample_CamRam
{
  class SimpleExample_CamRam
  {

    static int bcd_counter(Byte[] bcds)
    {
      return (int)((bcds[0]) / 0x10) * 10 + ((bcds[0]) % 0x10)
        + 100 * (int)((bcds[1]) / 0x10) * 10 + ((bcds[1]) % 0x10)
        + 10_000 * (int)((bcds[2]) / 0x10) * 10 + ((bcds[2]) % 0x10)
        + 1_000_000 * (int)((bcds[3]) / 0x10) * 10 + ((bcds[3]) % 0x10);
    }
    static void Main(string[] args)
    {
      uint err = 0;
      Image img = new Image();
      Camera cam = new Camera();
      ArrayList distribution = new ArrayList();
      distribution.Add(50.0);
      distribution.Add(20.0);
      distribution.Add(5.0);
      cam.setCamRamAllocation(distribution);
      cam.switchToCamRam();
      Configuration config = cam.getConfiguration();
      config.timestamp_mode = pco.sdk.PCO_SDK_DEFINES.TIMESTAMP_MODE_BINARYANDASCII;
      uint valid_images = cam.getCamRamNumImages();
      uint max_images = cam.getCamRamMaxImages();
      ushort segment = cam.getCamRamSegment();
      Image image_latest = new Image(config.roi, DataFormat.Mono16, RawFormat.UInt16);
      Image image_end = new Image(config.roi, DataFormat.Mono16, RawFormat.UInt16);


      Console.WriteLine("CamRam segment " + segment + ": " + valid_images + " / " + max_images + "\n");

      // segment
      Console.WriteLine("Record with CamRam segment " + segment + " as ring buffer ... \n");

      cam.record((int)cam.getCamRamMaxImages(), RecordMode.camram_ring);

      cam.waitForFirstImage();

      //Timer to get images for 10 seconds
      int i = 0;
      DateTime startTime = DateTime.Now;
      TimeSpan timeout = new TimeSpan(0,0,10);
      while (DateTime.Now >= (startTime + timeout))
      {
        cam.image(image_latest, pco.recorder.PCO_RECORDER_DEFINES.PCO_RECORDER_LATEST_IMAGE);
        Console.WriteLine("image " + i++ + ": " + image_latest.getRecorderImageNumber() + ", bcd: " + bcd_counter(image_latest.getMetaData().metaData.bIMAGE_COUNTER_BCD) + "\n");
        cam.waitForNewImage();
      }
      cam.stop();
      cam.image(image_latest, 0);
      cam.image(image_end, cam.getCamRamNumImages() - 1);
      Console.WriteLine("CamRam ring buffer holds images from " + bcd_counter(image_latest.getMetaData().metaData.bIMAGE_COUNTER_BCD) + " to " + bcd_counter(image_end.getMetaData().metaData.bIMAGE_COUNTER_BCD) + "\n");
      valid_images = cam.getCamRamNumImages();
      max_images = cam.getCamRamMaxImages();
      segment = cam.getCamRamSegment();

      Console.WriteLine("CamRam segment " + segment + ": " + valid_images + " / " + max_images + "\n");

      // segment
      Console.WriteLine("Record till CamRam segment " + segment + " is full ... \n");
      cam.record((int)cam.getCamRamMaxImages(), RecordMode.camram_segment);

      cam.waitForFirstImage();
      i = 0;
      while (cam.isRecording())
      {
        cam.image(image_latest, pco.recorder.PCO_RECORDER_DEFINES.PCO_RECORDER_LATEST_IMAGE);
        Console.WriteLine("image " + i++ + ": " + image_latest.getRecorderImageNumber() + ", bcd: " + bcd_counter(image_latest.getMetaData().metaData.bIMAGE_COUNTER_BCD) + "\n");
        cam.waitForNewImage();
      }

      cam.image(image_latest, 0);
      cam.image(image_end, cam.getCamRamNumImages() - 1);
      Console.WriteLine("CamRam ring buffer holds images from " + bcd_counter(image_latest.getMetaData().metaData.bIMAGE_COUNTER_BCD) + " to " + bcd_counter(image_end.getMetaData().metaData.bIMAGE_COUNTER_BCD) + "\n");
      valid_images = cam.getCamRamNumImages();
      max_images = cam.getCamRamMaxImages();
      segment = cam.getCamRamSegment();

      Console.WriteLine("CamRam segment " + segment + ": " + valid_images + " / " + max_images + "\n");

      if (err != 0)
      {
        throw new Camera_Exception(err);
      }

      //We suggest to actively close the cameras to avoid garbage collection errors
      cam.close();
    }
  }
}
