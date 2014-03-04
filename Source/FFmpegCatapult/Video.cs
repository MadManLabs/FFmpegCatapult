﻿﻿// Video properties interface for FFmpeg Catapult.
// Copyright (C) 2013 Myles Thaiss

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegCatapult
{
    class Video
    {
        // Variables
        public static bool UseCRF = false;
        private static int bFrames;
        private static int bFStrat;
        private static int bitrate;
        private static int bufferSize;
        private static int cmp;
        private static int codecLevel;
        private static int crf;
        private static int diaSize;
        private static int gopSize;
        private static int maxBitrate;
        private static int minBitrate;
        private static int qmax;
        private static int qmin;
        private static int subcmp;
        private static int trellis;
        private static string bits;
        private static string bytes;
        private static string codec;
        private static string[,] codecs;
        private static string codecProfile;
        private static string[,] codecProfiles = new string[,] {
            {"Baseline", "baseline"}, {"Main", "main"}, {"High", "high"}
        };
        private static string encoder;
        private static string[,] encoders;
        private static string encoderPreset;
        private static string[,] encoderPresets = new string[,] {
            {"Ultra Fast", "ultrafast"}, {"Super Fast", "superfast"},
            {"Very Fast", "veryfast"}, {"Faster", "faster"}, {"Fast", "fast"},
            {"Medium", "medium"}, {"Slow", "slow"}, {"Slower", "slower"},
            {"Very Slow", "veryslow"}, {"Placebo", "placebo"}
        };
        private static string[] cmpFuncs = new string[] {
            "sad", "sse", "satd", "dct", "psnr", "bit", "rd", "zero", "vsad",
            "vsse", "nsse", "w53", "w97", "dctmax", "chroma", "Default"
        };
        private static string meMethod;
        private static string[,] meMethods = new string[,] {
            {"Zero", "zero"}, {"Full", "full"}, {"EPZS", "epzs"}, {"Esa", "esa"},
            {"Tesa", "tesa"}, {"Dia", "dia"}, {"Log", "log"}, {"Phods", "phods"},
            {"X1", "x1"}, {"Hex", "hex"}, {"Umh", "umh"}, {"Iter", "iter"},
            {"Default", "default"}
        };
        private static string pictureFormat = "default";
        private static string[,] pictureFormats = new string[,] {
            {"YUV 4:2:0", "yuv420p"}, {"YUYV 4:2:2", "yuyv422"}, {"RGB 24", "rgb24"},
            {"BGR 24","bgr24"}, {"YUV 4:2:2", "yuv422p"}, {"YUV 4:4:4", "yuv44p"},
            {"YUV 4:1:0", "yuv410p"}, {"YUV 4:1:1", "yuv411p"}, {"Gray", "gray"},
            {"Default", "default"}
        };

        // Property methods
        public static int Bitrate
        {
            get { return bitrate; }
            set { bitrate = value; }
        }

        public static int BFrames
        {
            get { return bFrames; }
            set { bFrames = value; }
        }

        public static int BFStrategy
        {
            get { return bFStrat; }
            set { bFStrat = value; }
        }

        public static string Bits
        {
            get { return bits; }
            set { bits = value; }
        }

        public static int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        public static string Bytes
        {
            get { return bytes; }
            set { bytes = value; }
        }

        public static int CMP
        {
            get { return cmp; }
            set { cmp = value; }
        }

        public static string[] CMPFuncs
        {
            get { return cmpFuncs; }
        }

        public static string Codec
        {
            get { return codec; }
            set
            {
                codec = value;

                // Reset to default values
                UseCRF = false;
                bits = "k";
                bytes = "M";
                bFrames = 0;
                bFStrat = 3;
                bufferSize = 0;
                cmp = 15;
                diaSize = 0;
                gopSize = 0;
                maxBitrate = 0;
                meMethod = "default";
                minBitrate = 0;
                qmax = 0;
                qmin = 0;
                subcmp = 15;
                trellis = 3;

                // Init codec values
                switch (codec)
                {
                    case "dirac":
                        bitrate = 1500;
                        encoder = "libschroedinger";
                        encoders = new string[,] {
                            {"Dirac", "libschroedinger"}
                        };
                        break;
                    case "h264":
                        bitrate = 1000;
                        codecLevel = 31;
                        codecProfile = "main";
                        encoder = "libx264";
                        encoders = new string[,] {
                            {"x264", "libx264"}
                        };
                        encoderPreset = "medium";
                        break;
                    case "mpeg2":
                        bitrate = 4000;
                        encoder = "mpeg2video";
                        encoders = new string[,] {
                            {"MPEG-2 Video", "mpeg2video"}
                        };
                        break;
                    case "mpeg4":
                        bitrate = 1500;
                        codecProfile = "none";
                        encoder = "libxvid";
                        encoders = new string[,] {
                            {"MPEG-4 (FFmpeg)", "mpeg4"}, {"Xvid", "libxvid"}
                        };
                        break;
                    case "theora":
                        bitrate = 1800;
                        encoder = "libtheora";
                        encoders = new string[,] {
                            {"Theora", "libtheora"}
                        };
                        break;
                    case "vp8":
                        bitrate = 1500;
                        encoder = "libvpx";
                        encoders = new string[,] {
                            {"VPX", "libvpx"}
                        };
                        break;
                    case "wmv":
                        bitrate = 1500;
                        encoder = "wmv2";
                        encoders = new string[,] {
                            {"WMV 7 (wmv1)", "wmv1"}, {"WMV 8 (wmv2)", "wmv2"}
                        };
                        break;
                    default:
                        bitrate = 1500;
                        encoder = codec;
                        encoders = new string[,] {
                            {codec, codec}
                        };
                        break;
                }
            }
        }

        public static string[,] Codecs
        {
            get { return codecs; }
            set { codecs = value; }
        }

        public static int CodecLevel
        {
            get { return codecLevel; }
            set { codecLevel = value; }
        }

        public static string CodecProfile
        {
            get { return codecProfile; }
            set { codecProfile = value; }
        }

        public static string[,] CodecProfiles
        {
            get { return codecProfiles; }
        }

        public static int CRF
        {
            get { return crf; }
            set { crf = value; }
        }

        public static int DiaSize
        {
            get { return diaSize; }
            set { diaSize = value; }
        }

        public static string Encoder
        {
            get { return encoder; }
            set { encoder = value; }
        }

        public static string[,] Encoders
        {
            get { return encoders; }
        }

        public static string EncoderPreset
        {
            get { return encoderPreset; }
            set { encoderPreset = value; }
        }

        public static string[,] EncoderPresets
        {
            get { return encoderPresets; }
        }

        public static int GOPSize
        {
            get { return gopSize; }
            set { gopSize = value; }
        }

        public static int MaxBitrate
        {
            get { return maxBitrate; }
            set { maxBitrate = value; }
        }

        public static string MEMethod
        {
            get { return meMethod; }
            set { meMethod = value; }
        }

        public static string[,] MEMethods
        {
            get { return meMethods; }
        }

        public static int MinBitrate
        {
            get { return minBitrate; }
            set { minBitrate = value; }
        }

        public static string PictureFormat
        {
            get { return pictureFormat; }
            set { pictureFormat = value; }
        }

        public static string[,] PictureFormats
        {
            get { return pictureFormats; }
        }

        public static int Qmax
        {
            get { return qmax; }
            set { qmax = value; }
        }

        public static int Qmin
        {
            get { return qmin; }
            set { qmin = value; }
        }

        public static int SubCMP
        {
            get { return subcmp; }
            set { subcmp = value; }
        }

        public static int Trellis
        {
            get { return trellis; }
            set { trellis = value; }
        }
    }
}