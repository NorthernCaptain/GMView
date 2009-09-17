using System;
using System.Collections.Generic;
using System.Text;

namespace XnGFL
{
    /// <summary>
    /// Store EXIF data that we use in the application converted to the C-Sharp variables
    /// </summary>
    public class EXIFData: ncUtils.IRunnable
    {
        public static string exifToolExecutable = @"exiftool.exe";
        public static readonly string exifToolUpdateOptions = "-overwrite_original ";
        public static readonly string exifToolUpdateDateTimeOriginal = "-exif:datetimeoriginal=";
        public static readonly string exifToolUpdateGPSLatitude = "-exif:gpslatitude=";
        public static readonly string exifToolUpdateGPSLatitudeRef = "-exif:gpslatituderef="; //north or south
        public static readonly string exifToolUpdateGPSLongitude = "-exif:gpslongitude=";
        public static readonly string exifToolUpdateGPSLongitudeRef = "-exif:gpslongituderef="; //east or west
        public static readonly string exifToolUpdateGPSAltitude = "-exif:gpsaltitude=";
        public static readonly string exifToolUpdateGPSAltitudeRef = "-exif:gpsaltituderef="; //above sea level, under sea level, sea level

        /// <summary>
        /// Main IFDs used in EXIF
        /// </summary>
        public enum IFD
        {
            IFD_0 = 0x0001,
            EXIF_IFD = 0x0002,
            INTEROPERABILITY_IFD = 0x0004,
            IFD_THUMBNAIL = 0x0008,
            GPS_IFD = 0x0010,
            MAKERNOTE_IFD = 0x0020
        }

        /// <summary>
        /// EXIF tag that we want to recognize and use
        /// </summary>
        public enum Tags
        {
            IFD0_Image_Make = 0x010f, //Ascii 	The manufacturer of the recording equipment. This is the manufacturer of the DSC, scanner, video digitizer or other equipment that generated the image. When the field is left blank, it is treated as unknown.
            IFD0_Image_Model = 0x0110,//Ascii 	The model name or model number of the equipment. This is the model name or number of the DSC, scanner, video digitizer or other equipment that generated the image. When the field is left blank, it is treated as unknown.
            IFD0_Image_Orientation = 0x0112, //Short 	The image orientation viewed in terms of rows and columns.
            IFD0_Image_Artist = 0x013b, //Ascii 	This tag records the name of the camera owner, photographer or image creator. The detailed format is not specified, but it is recommended that the information be written as in the example below for ease of Interoperability. When the field is left blank, it is treated as unknown. Ex.) "Camera owner, John Smith; Photographer, Michael Brown; Image creator, Ken James"


            Photo_ExposureTime = 0x829a, //rational Exposure time, given in seconds (sec).
            Photo_FNumber = 0x829d, //rational
            Photo_ExposureProgram = 0x8822, //short
            Photo_ISOSpeedRatings = 0x8827, //short
            Photo_DateTimeOriginal = 0x9003, //date
            Photo_Flash = 0x9209, // short This tag is recorded when an image is taken using a strobe light (flash).
 	        Photo_FocalLength = 0x920a, //Rational 	The actual focal length of the lens, in mm. Conversion is not made to the focal length of a 35 mm film camera.        
            Photo_ExposureMode = 0xa402, //Short 	This tag indicates the exposure mode set when the image was shot. In auto-bracketing mode, the camera shoots a series of frames of the same scene at different exposure settings.
            Photo_ExposureCompensation = 0x9204, //SRational 	The exposure bias. The units is the APEX value. Ordinarily it is given in the range of -99.99 to 99.99.

            GPSInfo_GPSVersionID = 0x0000, //Byte 	Indicates the version of <GPSInfoIFD>. The version is given as 2.0.0.0. This tag is mandatory when <GPSInfo> tag is present. (Note: The <GPSVersionID> tag is given in bytes, unlike the <ExifVersion> tag. When the version is 2.0.0.0, the tag value is 02000000.H).
            GPSInfo_GPSLatitudeRef = 0x0001, //Ascii 	Indicates whether the latitude is north or south latitude. The ASCII value 'N' indicates north latitude, and 'S' is south latitude.
            GPSInfo_GPSLatitude = 0x0002, //Rational 	Indicates the latitude. The latitude is expressed as three RATIONAL values giving the degrees, minutes, and seconds, respectively. When degrees, minutes and seconds are expressed, the format is dd/1,mm/1,ss/1. When degrees and minutes are used and, for example, fractions of minutes are given up to two decimal places, the format is dd/1,mmmm/100,0/1.
            GPSInfo_GPSLongitudeRef = 0x0003, //Ascii 	Indicates whether the longitude is east or west longitude. ASCII 'E' indicates east longitude, and 'W' is west longitude.
            GPSInfo_GPSLongitude = 0x0004, //Rational 	Indicates the longitude. The longitude is expressed as three RATIONAL values giving the degrees, minutes, and seconds, respectively. When degrees, minutes and seconds are expressed, the format is ddd/1,mm/1,ss/1. When degrees and minutes are used and, for example, fractions of minutes are given up to two decimal places, the format is ddd/1,mmmm/100,0/1.
            GPSInfo_GPSAltitudeRef = 0x0005, //Byte 	Indicates the altitude used as the reference altitude. If the reference is sea level and the altitude is above sea level, 0 is given. If the altitude is below sea level, a value of 1 is given and the altitude is indicated as an absolute value in the GSPAltitude tag. The reference unit is meters. Note that this tag is BYTE type, unlike other reference tags.
            GPSInfo_GPSAltitude = 0x0006, //Rational 	Indicates the altitude based on the reference in GPSAltitudeRef. Altitude is expressed as one RATIONAL value. The reference unit is meters.
        }

        private string fname;

        /// <summary>
        /// File name of this EXIF data.
        /// </summary>
        public string filename
        {
            get { return fname; }
        }

        private bool modified = false;

        /// <summary>
        /// Return true if data has been modified
        /// </summary>
        public bool isModified
        {
            get { return modified; }
        }

        private DateTime vDateTimeOriginal = DateTime.Now;

        /// <summary>
        /// Original date and time - time of the shot
        /// </summary>
        public DateTime dateTimeOriginal
        {
            get { return vDateTimeOriginal; }
            set 
            {
                if (vDateTimeOriginal.Equals(value))
                    return;
                vDateTimeOriginal = value; 
                modified = true; 
            }
        }


        private string vExposureInfo = "";
        /// <summary>
        /// Return exposure information: ExposureTime sec at Aperture, Focal Length
        /// </summary>
        public string exposureInfo
        {
            get { return vExposureInfo; }
        }

        private string vFlash = "";
        /// <summary>
        /// Return flash information.
        /// </summary>
        public string flash
        {
            get { return vFlash; }
        }

        /// <summary>
        /// Return GPS info version
        /// </summary>
        public string gpsVersion
        {
            get { return getTagString(IFD.GPS_IFD, Tags.GPSInfo_GPSVersionID); }
            set 
            {
                if (has_gps)
                    items[makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSVersionID)] = value;
                else
                    items.Add(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSVersionID), value); 
                has_gps = true;
            }
        }

        /// <summary>
        /// Tags list and their conversion methods
        /// </summary>
        private Dictionary<int, convertDataDelegate> tagsInUse = new Dictionary<int, convertDataDelegate>();
        
        /// <summary>
        /// Stores tags and their string values
        /// </summary>
        private Dictionary<int, string> items = new Dictionary<int, string>();

        private delegate void convertDataDelegate(MetaDataWrap.GFL_EXIF_ENTRY entry);


        private void cnvString(MetaDataWrap.GFL_EXIF_ENTRY entry)
        {
            int key = makeKeyInt((int)entry.Flag, (int)entry.Tag);
            items.Add(key, entry.Value);
        }

        private void cnvDateTimeOriginal(MetaDataWrap.GFL_EXIF_ENTRY entry)
        {
            cnvString(entry);
            vDateTimeOriginal = EXIFUtil.ParseDateTime(entry.Value);
        }


        private double gps_lon = 0;
        private double gps_lat = 0;
        private double gps_alt = 0;

        private string gps_lon_string = string.Empty;
        private string gps_lat_string = string.Empty;
        private string gps_alt_string = string.Empty;

        private bool has_gps = false;

        /// <summary>
        /// Do we have GPS information here
        /// </summary>
        public bool hasGPS
        {
            get { return has_gps; }
        }

        /// <summary>
        /// Has data been modified or no.
        /// </summary>
        public bool hasModified
        {
            get { return modified; }
        }

        /// <summary>
        /// Gets or sets GPS longitude
        /// </summary>
        public double gpsLon
        {
            get { return gps_lon; }
            set 
            {
                if (ncGeo.CommonGeo.almostEqual(gps_lon, value))
                    return;

                gps_lon = value; 
                gps_lon_string = ncUtils.Glob.lonString(gps_lon); 
                modified = true; 
            }
        }

        /// <summary>
        /// Gets or sets GPS latitude
        /// </summary>
        public double gpsLat
        {
            get { return gps_lat;}
            set 
            {
                if (ncGeo.CommonGeo.almostEqual(gps_lat, value))
                    return;
                gps_lat = value; 
                gps_lat_string = ncUtils.Glob.latString(gps_lat); 
                modified = true; 
            }
        }

        /// <summary>
        /// Gets or sets GPS altitude
        /// </summary>
        public double gpsAlt
        {
            get { return gps_alt; }
            set
            {
                if (ncGeo.CommonGeo.almostEqual(gps_alt, value))
                    return;
                gps_alt = value;
                gps_alt_string = gps_alt.ToString("F3");
                modified = true;
            }
        }

        /// <summary>
        /// String representation of lon value
        /// </summary>
        public string gpsLonSting
        {
            get { return gps_lon_string; }
        }

        /// <summary>
        /// String representation of lat value
        /// </summary>
        public string gpsLatString
        {
            get { return gps_lat_string; }
        }

        /// <summary>
        /// String representation of altitude value
        /// </summary>
        public string gpsAltString
        {
            get { return gps_alt_string; }
        }

        /// <summary>
        /// Sets all gps coordinates at once
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="alt"></param>
        public void setGPS(double lon, double lat, double alt)
        {
            if (!hasGPS)
            {
                gpsVersion = "2.2.0.0";
            }
            gpsLon = lon;
            gpsLat = lat;
            gpsAlt = alt;
        }

        /// <summary>
        /// Initializes dictionary with methods for converting parameters.
        /// </summary>
        private void initMap()
        {
            tagsInUse.Add(makeKey(IFD.IFD_0, Tags.IFD0_Image_Artist), cnvString);
            tagsInUse.Add(makeKey(IFD.IFD_0, Tags.IFD0_Image_Make), cnvString);
            tagsInUse.Add(makeKey(IFD.IFD_0, Tags.IFD0_Image_Model), cnvString);
            tagsInUse.Add(makeKey(IFD.EXIF_IFD, Tags.Photo_ExposureTime), cnvString);
            tagsInUse.Add(makeKey(IFD.EXIF_IFD, Tags.Photo_FNumber), cnvString);
            tagsInUse.Add(makeKey(IFD.EXIF_IFD, Tags.Photo_FocalLength), cnvString);
            tagsInUse.Add(makeKey(IFD.EXIF_IFD, Tags.Photo_ISOSpeedRatings), cnvString);
            tagsInUse.Add(makeKey(IFD.EXIF_IFD, Tags.Photo_Flash), cnvString);
            tagsInUse.Add(makeKey(IFD.EXIF_IFD, Tags.Photo_ExposureProgram), cnvString);
            tagsInUse.Add(makeKey(IFD.EXIF_IFD, Tags.Photo_ExposureCompensation), cnvString);
            tagsInUse.Add(makeKey(IFD.EXIF_IFD, Tags.Photo_DateTimeOriginal), cnvDateTimeOriginal);

            tagsInUse.Add(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSVersionID), cnvString);
            tagsInUse.Add(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSLatitudeRef), cnvString);
            tagsInUse.Add(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSLatitude), cnvString);
            tagsInUse.Add(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSLongitudeRef), cnvString);
            tagsInUse.Add(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSLongitude), cnvString);
            tagsInUse.Add(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSAltitude), cnvString);
            tagsInUse.Add(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSAltitudeRef), cnvString);
        }

        /// <summary>
        /// Return string representation of the known tag
        /// </summary>
        /// <param name="ifd"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string getTagString(IFD ifd, Tags tag)
        {
            string result;
            if (items.TryGetValue(makeKey(ifd, tag), out result))
                return result;
            return string.Empty;
        }

        /// <summary>
        /// Make Key int from the given ifd and tag
        /// </summary>
        /// <param name="ifd"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static int makeKey(EXIFData.IFD ifd, EXIFData.Tags tag)
        {
            return (((int)ifd) << 16) | (int)tag;
        }

        public static int makeKeyInt(int ifd, int tag)
        {
            return (ifd << 16) | tag;
        }

        /// <summary>
        /// Constructor that initializes needed data from exif context
        /// </summary>
        /// <param name="gflExifContext"></param>
        public EXIFData(string fname, MetaDataWrap.XnMetaContext gflExifContext)
        {
            this.fname = fname;

            initMap();

            loadFromCtx(gflExifContext);

            try
            {
                vExposureInfo = items[makeKey(IFD.EXIF_IFD, Tags.Photo_ExposureTime)];

                vExposureInfo += " sec at ƒ/" + items[makeKey(IFD.EXIF_IFD, Tags.Photo_FNumber)];
                { //Exposure Compensation
                    string ec;
                    if (items.TryGetValue(makeKey(IFD.EXIF_IFD, Tags.Photo_ExposureCompensation), out ec))
                    {
                        if (ec[0] != '0')
                        {
                            if (ec[0] != '-')
                                vExposureInfo += " +" + ec + " EV";
                            else
                                vExposureInfo += " " + ec + " EV";
                        }
                    }
                }

                vExposureInfo += ", " + items[makeKey(IFD.EXIF_IFD, Tags.Photo_FocalLength)];
                vExposureInfo += " mm, ISO" + items[makeKey(IFD.EXIF_IFD, Tags.Photo_ISOSpeedRatings)];

                vFlash = items[makeKey(IFD.EXIF_IFD, Tags.Photo_ExposureProgram)] + ", " +
                        items[makeKey(IFD.EXIF_IFD, Tags.Photo_Flash)];
            }
            catch { };

            { //GPS
                string val;
                if (items.TryGetValue(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSLongitude), out val))
                {
                    try
                    {
                        gps_lon = EXIFUtil.EXIFtoGPSDegrees(val);
                        if (items.TryGetValue(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSLongitudeRef), out val)
                            && val.Equals("West"))
                            gps_lon = -gps_lon;
                        gps_lon_string = ncUtils.Glob.lonString(gps_lon);
                    }
                    catch (Exception ex)
                    {
                        gps_lon_string = "ERR:" + ex.Message;
                    }

                    if (items.TryGetValue(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSLatitude), out val))
                    {
                        try
                        {
                            gps_lat = EXIFUtil.EXIFtoGPSDegrees(val);
                            if (items.TryGetValue(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSLatitudeRef), out val)
                                && val.Equals("South"))
                                gps_lat = -gps_lat;
                            gps_lat_string = ncUtils.Glob.latString(gps_lat);
                        }
                        catch (Exception ex2)
                        {
                            gps_lat_string = "ERR:" + ex2.Message;
                        }

                        has_gps = true;
                    }

                    if (items.TryGetValue(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSAltitude), out val))
                    {
                        try
                        {
                            gps_alt = Double.Parse(val, ncUtils.Glob.numformat);
                            if (items.TryGetValue(makeKey(IFD.GPS_IFD, Tags.GPSInfo_GPSAltitudeRef), out val)
                                && val.Equals("Other"))
                            {
                                gps_alt = -gps_alt;
                            }
                            gps_alt_string = gps_alt.ToString("F3");
                        }
                        catch (Exception ex3)
                        {
                            gps_alt_string = "ERR:" + ex3.Message;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Do we have exif information or no?
        /// </summary>
        public bool hasEXIF
        {
            get { return items.Count != 0; }
        }

        /// <summary>
        /// Loads data from gfl EXIF context wrapper
        /// </summary>
        /// <param name="ctx"></param>
        private void loadFromCtx(MetaDataWrap.XnMetaContext ctx)
        {
            if (MetaDataWrap.xnHasEXIF(ctx) != 0)
            {
                MetaDataWrap.GFL_EXIF_ENTRY item = MetaDataWrap.xnStartEXIF1(ctx);
                while (item != null)
                {
                    convertDataDelegate func;
                    if(tagsInUse.TryGetValue(makeKeyInt((int)item.Flag, (int)item.Tag), out func))
                        func(item);

                    item = MetaDataWrap.xnNextEXIF1(ctx);
                }
            }
        }

        #region IRunnable Members

        /// <summary>
        /// Here we do update of our file if we have changes in the exif data (isModified)
        /// </summary>
        public void run()
        {
            if (!isModified)
                return;

            string parameters = exifToolUpdateOptions + " \"" + exifToolUpdateDateTimeOriginal
                + EXIFUtil.FormatDateTime(dateTimeOriginal) + "\" \"";

            {
                //GPS construction
                parameters += exifToolUpdateGPSLatitude + Math.Abs(gps_lat).ToString("F6", ncUtils.Glob.numformat) + "\" \""
                    + exifToolUpdateGPSLatitudeRef + (gps_lat < 0.0 ? "south\" \"" : "north\" \"");

                parameters += exifToolUpdateGPSLongitude + Math.Abs(gps_lon).ToString("F6", ncUtils.Glob.numformat) + "\" \""
                    + exifToolUpdateGPSLongitudeRef + (gps_lon < 0.0 ? "west\" \"" : "east\" \"");

                parameters += exifToolUpdateGPSAltitude + Math.Abs(gps_alt).ToString("F3", ncUtils.Glob.numformat) + "\" \""
                    + exifToolUpdateGPSAltitudeRef + (gps_alt < 0.0 ? "below sea level\" \"" : "above sea level\" \"");
            }

            parameters += filename + "\"";

            object result = ncUtils.ShellExec.ExecuteCommandSyncNoCMD(exifToolExecutable, parameters);
            //We done our update
            if (result is string)
                modified = false;
        }

        #endregion
    }
}
