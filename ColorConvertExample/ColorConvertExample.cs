using pco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorConvertExample
{
  class ColorConvertExample
  {
    static void Main(string[] args)
    {
      uint err = 0;
      string filename = "example.tif";
      string saveType = pco.recorder.PCO_RECORDER_DEFINES.FILESAVE_IMAGE_BGR_8;
      Image img = new Image();
      int image_count = 10;
      Camera cam = new Camera();

      if (!cam.isColored())
      {
        string lut_file_default = Directory.GetCurrentDirectory() + "\\..\\..\\lut\\LUT_rainbow.lt1";
        Console.WriteLine("Enter filepath to LUT file \n(default: " + lut_file_default + "):");
        string lut_file = Console.ReadLine();
        if (lut_file == null || lut_file.Length == 0)
          lut_file = lut_file_default;
        if (!File.Exists(lut_file))
          throw new pco.Camera_Exception("path to LUT file is invalid.");

        ConvertControlPseudoColor cc = (ConvertControlPseudoColor)(cam.getConvertControl(DataFormat.BGR8));
        cc.lut_file = lut_file;
        cam.setConvertControl(DataFormat.BGR8, cc);
      }

      cam.record(image_count, RecordMode.sequence);
      for (uint counter = 0; counter < image_count; counter++)
      {
        cam.image(img, counter, null, DataFormat.BGR8);
        err = pco.recorder.PCO_RECORDER.PCO_RecorderSaveImage(img.vector_8bit(), (UInt16)img.width(), (UInt16)img.height(), saveType, false, "" + counter + "_" + filename, true, IntPtr.Zero);
        if (err != 0)
          throw new Camera_Exception(err);
        Console.WriteLine("Image Count " + counter + " > " + counter + "_" + filename);
      }
      //We suggest to actively close the cameras to avoid garbage collection errors
      cam.close();
    }
  }
}
