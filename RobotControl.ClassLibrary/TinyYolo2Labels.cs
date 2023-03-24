namespace RobotControl.ClassLibrary
{
    public static class TinyYolo2Labels
    {
        public static string[] Labels => labels;

        private static string[] labels = new string[] {
                "aeroplane"  , "bicycle", "bird" , "boat"     , "bottle"   ,
                "bus"        , "car"    , "cat"  , "chair"    , "cow"      ,
                "diningtable", "dog"    , "horse", "motorbike", "person"   ,
                "pottedplant", "sheep"  , "sofa" , "train"    , "tvmonitor"
        };
    }
}
