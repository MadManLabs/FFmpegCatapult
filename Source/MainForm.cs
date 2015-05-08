﻿﻿// Main Windows Form logic methods for FFmpeg Catapult
// Copyright (C) 2015 Myles Thaiss

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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace FFmpegCatapult
{
    public partial class MainForm : Form
    {
        // Variables
        private String fileContainer;
        private String fileExtension;

        public MainForm()
        {
            InitializeComponent();
        }

        // Methods
        private void InitTabs()
        {
            // Picture tab
            if (Methods.IsPictureScalable())
            {
                tabPicture.Enabled = true;
            }
            else
            {
                tabPicture.Enabled = false;
            }

            // Video tab
            if (Methods.IsAudioFile())
            {
                tabVideo.Enabled = false;
            }
            else
            {
                tabVideo.Enabled = true;
            }
        }

        private void InitMain()
        {
            // Combo boxes
            comboBoxContainers.SelectedIndexChanged -= new EventHandler(comboBoxContainers_SelectedIndexChanged);
            for (int i = 0; i < File.Formats.GetLength(0); i++)
            {
                if (File.Formats[i, 1] == File.Format)
                {
                    comboBoxContainers.SelectedIndex = i;
                    fileContainer = File.Formats[i, 0];
                    fileExtension = File.Formats[i, 1];
                    break;
                }
            }
            comboBoxContainers.SelectedIndexChanged += new EventHandler(comboBoxContainers_SelectedIndexChanged);

            comboBoxThreads.SelectedIndexChanged -= new EventHandler(comboBoxThreads_SelectedIndexChanged);
            comboBoxThreads.SelectedIndex = Session.Threads;
            comboBoxThreads.SelectedIndexChanged += new EventHandler(comboBoxThreads_SelectedIndexChanged);

            // Text boxes
            if (textBoxOutFile.Text != "")
            {
                textBoxOutFile.TextChanged -= new EventHandler(textBoxOutFile_TextChanged);
                File.Output = textBoxOutFile.Text;
                textBoxOutFile.Text = File.Output;
                textBoxOutFile.TextChanged += new EventHandler(textBoxOutFile_TextChanged);
            }

            // Checkboxes
            checkBoxMultiThreading.CheckedChanged -= new EventHandler(checkBoxMultiThreading_CheckedChanged);
            checkBoxMultiThreading.Checked = Session.MultiThreading;
            comboBoxThreads.Enabled = Session.MultiThreading;
            checkBoxMultiThreading.CheckedChanged += new EventHandler(checkBoxMultiThreading_CheckedChanged);
        }

        private void InitPicture()
        {
            // Resolution radio buttons
            radioButtonKeepRes.CheckedChanged -= new EventHandler(radioButtonKeepRes_CheckedChanged);
            radioButtonHalfRes.CheckedChanged -= new EventHandler(radioButtonHalfRes_CheckedChanged);
            radioButtonCustomRes.CheckedChanged -= new EventHandler(radioButtonCustomRes_CheckedChanged);
            if (Screen.ScaleOption == 0)
            {
                radioButtonKeepRes.Checked = true;
                EnableResControls(false);
            }
            else if (Screen.ScaleOption == 1)
            {
                radioButtonCustomRes.Checked = true;
                EnableResControls(true);
            }
            else
            {
                radioButtonHalfRes.Checked = true;
                EnableResControls(false);
            }
            radioButtonKeepRes.CheckedChanged += new EventHandler(radioButtonKeepRes_CheckedChanged);
            radioButtonHalfRes.CheckedChanged += new EventHandler(radioButtonHalfRes_CheckedChanged);
            radioButtonCustomRes.CheckedChanged += new EventHandler(radioButtonCustomRes_CheckedChanged);

            // Check boxes
            checkBoxAspectRatio.CheckedChanged -= new EventHandler(checkBoxAspectRatio_CheckedChanged);
            checkBoxAspectRatio.Checked = Screen.AspectRatio;
            EnableRatioControls(Screen.AspectRatio);
            checkBoxAspectRatio.CheckedChanged += new EventHandler(checkBoxAspectRatio_CheckedChanged);

            checkBoxCrop.CheckedChanged += new EventHandler(checkBoxCrop_CheckedChanged);
            checkBoxCrop.Checked = Screen.CropVideo;
            EnableLayoutControls(Screen.CropVideo);
            checkBoxCrop.CheckedChanged += new EventHandler(checkBoxCrop_CheckedChanged);

            checkBoxDeinterlace.CheckedChanged -= new EventHandler(checkBoxDeinterlace_CheckedChanged);
            checkBoxDeinterlace.Checked = Screen.Deinterlace;
            checkBoxDeinterlace.CheckedChanged += new EventHandler(checkBoxDeinterlace_CheckedChanged);

            checkBoxPad.CheckedChanged -= new EventHandler(checkBoxPad_CheckedChanged);
            checkBoxPad.Checked = Screen.PadVideo;
            EnableLayoutControls(Screen.PadVideo);
            checkBoxPad.CheckedChanged += new EventHandler(checkBoxPad_CheckedChanged);

            // Text boxes
            textBoxWidth.TextChanged -= new EventHandler(textBoxWidth_TextChanged);
            textBoxWidth.Text = Methods.NumToText(Screen.Width);
            textBoxWidth.TextChanged += new EventHandler(textBoxWidth_TextChanged);

            textBoxHeight.TextChanged -= new EventHandler(textBoxHeight_TextChanged);
            textBoxHeight.Text = Methods.NumToText(Screen.Height);
            textBoxHeight.TextChanged += new EventHandler(textBoxHeight_TextChanged);

            textBoxRatioA.TextChanged -= new EventHandler(textBoxRatioA_TextChanged);
            textBoxRatioA.Text = Methods.NumToText(Screen.RatioA);
            textBoxRatioA.TextChanged += new EventHandler(textBoxRatioA_TextChanged);

            textBoxRatioB.TextChanged -= new EventHandler(textBoxRatioB_TextChanged);
            textBoxRatioB.Text = Methods.NumToText(Screen.RatioB);
            textBoxRatioB.TextChanged += new EventHandler(textBoxRatioB_TextChanged);

            textBoxFPS.TextChanged -= new EventHandler(textBoxFPS_TextChanged);
            textBoxFPS.Text = Methods.NumToText(Screen.FPS);
            textBoxFPS.TextChanged += new EventHandler(textBoxFPS_TextChanged);

            textBoxLayoutWidth.TextChanged -= new EventHandler(textBoxLayoutWidth_TextChanged);
            textBoxLayoutWidth.Text = Methods.NumToText(Screen.WinWidth);
            textBoxLayoutWidth.TextChanged += new EventHandler(textBoxLayoutWidth_TextChanged);

            textBoxLayoutHeight.TextChanged -= new EventHandler(textBoxLayoutHeight_TextChanged);
            textBoxLayoutHeight.Text = Methods.NumToText(Screen.WinHeight);
            textBoxLayoutHeight.TextChanged += new EventHandler(textBoxLayoutHeight_TextChanged);

            textBoxLayoutVert.TextChanged -= new EventHandler(textBoxLayoutVert_TextChanged);
            textBoxLayoutVert.Text = Methods.NumToText(Screen.X);
            textBoxLayoutVert.TextChanged += new EventHandler(textBoxLayoutVert_TextChanged);

            textBoxLayoutHoriz.TextChanged -= new EventHandler(textBoxLayoutHoriz_TextChanged);
            textBoxLayoutHoriz.Text = Methods.NumToText(Screen.Y);
            textBoxLayoutHoriz.TextChanged += new EventHandler(textBoxLayoutHoriz_TextChanged);

            // Set selected scaling method
            comboBoxScalingMethods.SelectedIndexChanged -= new EventHandler(comboBoxScalingMethods_SelectedIndexChanged);
            for (int i = 0; i < Screen.ScalingMethods.GetLength(0); i++)
            {
                if (Screen.ScalingMethod == Screen.ScalingMethods[i, 1])
                {
                    comboBoxScalingMethods.SelectedIndex = i;
                    break;
                }
            }
            comboBoxScalingMethods.SelectedIndexChanged += new EventHandler(comboBoxScalingMethods_SelectedIndexChanged);
        }

        private void InitVideo()
        {
            comboBoxVideoCodecs.Items.Clear();
            for (int i = 0; i < Video.Codecs.GetLength(0); i++)
            {
                comboBoxVideoCodecs.Items.Add(new Methods.ListComboContent(Video.Codecs[i, 0], Video.Codecs[i, 1]));
            }

            if (Video.Codec == "copy" | Video.Codec == "none")
            {
                EnableVideoControls(false);
            }
            else
            {
                EnableVideoControls(true);
            }

            // Combo boxes
            comboBoxVideoCodecs.SelectedIndexChanged -= new EventHandler(comboBoxVideoCodecs_SelectedIndexChanged);
            for (int i = 0; i < Video.Codecs.GetLength(0); i++)
            {
                if (Video.Codec == Video.Codecs[i, 1])
                {
                    comboBoxVideoCodecs.SelectedIndex = i;
                    break;
                }
            }
            comboBoxVideoCodecs.SelectedIndexChanged += new EventHandler(comboBoxVideoCodecs_SelectedIndexChanged);

            comboBoxVideoEncoders.SelectedIndexChanged -= new EventHandler(comboBoxVideoEncoders_SelectedIndexChanged);
            comboBoxVideoEncoders.Items.Clear();
            for (int i = 0; i < Video.Encoders.GetLength(0); i++)
            {
                comboBoxVideoEncoders.Items.Add(new Methods.ListComboContent(Video.Encoders[i, 0], Video.Encoders[i, 1]));

                if (Video.Encoder == Video.Encoders[i, 1])
                {
                    comboBoxVideoEncoders.SelectedIndex = i;
                }
            }
            comboBoxVideoEncoders.SelectedIndexChanged += new EventHandler(comboBoxVideoEncoders_SelectedIndexChanged);

            comboBoxBits.SelectedIndexChanged -= new EventHandler(comboBoxBits_SelectedIndexChanged);
            if (Video.Bits == "k")
            {
                comboBoxBits.SelectedIndex = 0;
            }
            else
            {
                comboBoxBits.SelectedIndex = 1;
            }
            labelMinBits.Text = comboBoxBits.Text;
            labelMaxBits.Text = comboBoxBits.Text;
            comboBoxBits.SelectedIndexChanged += new EventHandler(comboBoxBits_SelectedIndexChanged);

            comboBoxBytes.SelectedIndexChanged -= new EventHandler(comboBoxBytes_SelectedIndexChanged);
            if (Video.Bytes == "k")
            {
                comboBoxBytes.SelectedIndex = 0;
            }
            else if (Video.Bytes == "M")
            {
                comboBoxBytes.SelectedIndex = 1;
            }
            else
            {
                comboBoxBytes.SelectedIndex = 2;
            }
            comboBoxBytes.SelectedIndexChanged += new EventHandler(comboBoxBytes_SelectedIndexChanged);

            // Check boxes
            checkBoxTwoPassEncoding.CheckedChanged -= new EventHandler(checkBoxTwoPassEncoding_CheckedChanged);
            checkBoxTwoPassEncoding.Checked = Session.TwoPassEncoding;
            checkBoxTwoPassEncoding.CheckedChanged += new EventHandler(checkBoxTwoPassEncoding_CheckedChanged);

            checkBoxUseCRF.CheckedChanged -= new EventHandler(checkBoxUseCRF_CheckedChanged);
            checkBoxUseCRF.Checked = Video.UseCRF;
            EnableCRFControls(Video.UseCRF);
            checkBoxUseCRF.CheckedChanged += new EventHandler(checkBoxUseCRF_CheckedChanged);

            // Text boxes
            textBoxVideoBitrate.TextChanged -= new EventHandler(textBoxVideoBitrate_TextChanged);
            textBoxVideoBitrate.Text = Methods.NumToText(Video.Bitrate);
            textBoxVideoBitrate.TextChanged += new EventHandler(textBoxVideoBitrate_TextChanged);

            textBoxMinBitrate.TextChanged -= new EventHandler(textBoxMinBitrate_TextChanged);
            textBoxMinBitrate.Text = Methods.NumToText(Video.MinBitrate);
            textBoxMinBitrate.TextChanged += new EventHandler(textBoxMinBitrate_TextChanged);

            textBoxMaxBitrate.TextChanged -= new EventHandler(textBoxMaxBitrate_TextChanged);
            textBoxMaxBitrate.Text = Methods.NumToText(Video.MaxBitrate);
            textBoxMaxBitrate.TextChanged += new EventHandler(textBoxMaxBitrate_TextChanged);

            textBoxBufferSize.TextChanged -= new EventHandler(textBoxBufferSize_TextChanged);
            textBoxBufferSize.Text = Methods.NumToText(Video.BufferSize);
            textBoxBufferSize.TextChanged += new EventHandler(textBoxBufferSize_TextChanged);

            textBoxCRF.TextChanged -= new EventHandler(textBoxCRF_TextChanged);
            textBoxCRF.Text = Methods.NumToText(Video.CRF);
            textBoxCRF.TextChanged += new EventHandler(textBoxCRF_TextChanged);

            textBoxQmax.TextChanged -= new EventHandler(textBoxQmax_TextChanged);
            textBoxQmax.Text = Methods.NumToText(Video.Qmax);
            textBoxQmax.TextChanged += new EventHandler(textBoxQmax_TextChanged);

            textBoxQmin.TextChanged -= new EventHandler(textBoxQmin_TextChanged);
            textBoxQmin.Text = Methods.NumToText(Video.Qmin);
            textBoxQmin.TextChanged += new EventHandler(textBoxQmin_TextChanged);
        }

        private void InitAudio()
        {
            comboBoxAudioCodecs.Items.Clear();
            for (int i = 0; i < Audio.Codecs.GetLength(0); i++)
            {
                comboBoxAudioCodecs.Items.Add(new Methods.ListComboContent(Audio.Codecs[i, 0], Audio.Codecs[i, 1]));
            }

            if (Audio.Codec == "copy" | Audio.Codec == "none")
            {
                EnableAudioControls(false);

                if (Audio.Codec == "copy")
                {
                    groupBoxAudioStream.Enabled = true;
                }
                else
                {
                    groupBoxAudioStream.Enabled = false;
                }
            }
            else
            {
                EnableAudioControls(true);
                groupBoxAudioStream.Enabled = true;
            }

            // Combo boxes
            comboBoxAudioCodecs.SelectedIndexChanged -= new EventHandler(comboBoxAudioCodecs_SelectedIndexChanged);
            for (int i = 0; i < Audio.Codecs.GetLength(0); i++)
            {
                if (Audio.Codec == Audio.Codecs[i, 1])
                {
                    comboBoxAudioCodecs.SelectedIndex = i;
                }
            }
            comboBoxAudioCodecs.SelectedIndexChanged += new EventHandler(comboBoxAudioCodecs_SelectedIndexChanged);

            comboBoxAudioEncoders.SelectedIndexChanged -= new EventHandler(comboBoxAudioEncoders_SelectedIndexChanged);
            comboBoxAudioEncoders.Items.Clear();
            for (int i = 0; i < Audio.Encoders.GetLength(0); i++)
            {
                comboBoxAudioEncoders.Items.Add(new Methods.ListComboContent(Audio.Encoders[i, 0], Audio.Encoders[i, 1]));

                if (Audio.Encoder == Audio.Encoders[i, 1])
                {
                    comboBoxAudioEncoders.SelectedIndex = i;
                }
            }
            comboBoxAudioEncoders.SelectedIndexChanged += new EventHandler(comboBoxAudioEncoders_SelectedIndexChanged);

            comboBoxSampleRates.SelectedIndexChanged -= new EventHandler(comboBoxSampleRates_SelectedIndexChanged);
            comboBoxSampleRates.Items.Clear();

            for (int i = 0; i < Audio.SampleRates.GetLength(0); i++)
            {
                comboBoxSampleRates.Items.Add(new Methods.ListComboContent(Convert.ToString(Audio.SampleRates[i]) + " Hz", Audio.SampleRates[i]));
            }
            comboBoxSampleRates.Items.Add(new Methods.ListComboContent("", 0));

            if (Audio.SampleRate != 0)
            {
                for (int i = 0; i < Audio.SampleRates.GetLength(0); i++)
                {
                    if (Audio.SampleRate == Audio.SampleRates[i])
                    {
                        comboBoxSampleRates.SelectedIndex = i;
                        break;
                    }
                }
            }

            comboBoxSampleRates.SelectedIndexChanged += new EventHandler(comboBoxSampleRates_SelectedIndexChanged);

            comboBoxChannels.SelectedIndexChanged -= new EventHandler(comboBoxChannels_SelectedIndexChanged);
            comboBoxChannels.Items.Clear();

            for (int i = 1; i <= Audio.MaxChannels; i++)
            {
                comboBoxChannels.Items.Add(new Methods.ListComboContent(Convert.ToString(i), i));
            }
            comboBoxChannels.Items.Add(new Methods.ListComboContent("", 0));

            if (Audio.Channels != 0)
            {
                comboBoxChannels.SelectedIndex = Audio.Channels - 1;
            }
            comboBoxChannels.SelectedIndexChanged += new EventHandler(comboBoxChannels_SelectedIndexChanged);

            // Other items
            InitAudioEncPropertiesButton();
            InitAudioBitrates();
        }

        private void InitAudioEncPropertiesButton()
        {
            if (Audio.Encoder == "libfdk_aac")
            {
                buttonAudioCodecProperties.Enabled = true;
            }
            else
            {
                buttonAudioCodecProperties.Enabled = false;
            }
        }

        private void InitAudioBitrates()
        {
            // Check boxes
            checkBoxUseAudioVBR.CheckedChanged -= new EventHandler(checkBoxUseAudioVBR_CheckedChanged);
            checkBoxUseAudioVBR.Visible = Audio.VBRSupported;
            checkBoxUseAudioVBR.Checked = Audio.UseVBR;
            checkBoxUseAudioVBR.CheckedChanged += new EventHandler(checkBoxUseAudioVBR_CheckedChanged);

            // Combo boxes
            comboBoxAudioBitrates.SelectedIndexChanged -= new EventHandler(comboBoxAudioBitrates_SelectedIndexChanged);
            comboBoxAudioBitrates.Items.Clear();

            if (Audio.UseVBR == true)
            {
                // VBR modes
                labelAudioBitrate.Text = "Quality:";

                for (int i = 0; i < Audio.VBRModes.GetLength(0); i++)
                {
                    comboBoxAudioBitrates.Items.Add(new Methods.ListComboContent(Convert.ToString(Audio.VBRModes[i]), Audio.VBRModes[i]));
                }
                for (int i = 0; i < Audio.VBRModes.GetLength(0); i++)
                {
                    if (Audio.Quality == Audio.VBRModes[i])
                    {
                        comboBoxAudioBitrates.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                // ABR and CBR bitrates
                labelAudioBitrate.Text = "Bitrate:";

                for (int i = 0; i < Audio.Bitrates.GetLength(0); i++)
                {
                    comboBoxAudioBitrates.Items.Add(new Methods.ListComboContent(Convert.ToString(Audio.Bitrates[i]) + " Kbps", Audio.Bitrates[i]));
                }
                for (int i = 0; i < Audio.Bitrates.GetLength(0); i++)
                {
                    if (Audio.Bitrate == Audio.Bitrates[i])
                    {
                        comboBoxAudioBitrates.SelectedIndex = i;
                        break;
                    }
                }
            }

            comboBoxAudioBitrates.SelectedIndexChanged += new EventHandler(comboBoxAudioBitrates_SelectedIndexChanged);
        }

        private void InitMetadata()
        {
            if (File.Format == "raw" | File.Format == "mpg")
            {
                EnableTaggingControls(false);
            }
            else
            {
                EnableTaggingControls(true);

                switch (File.Format)
                {
                    case "avi":
                        EnableAlbumTagging(true);
                        EnableAlbumArtistTagging(false);
                        EnableArtistTagging(true);
                        EnableCommentTagging(true);
                        EnableDiscTagging(false);
                        EnableGenreTaggin(true);
                        EnablePublisherTagging(false);
                        EnableTitleTagging(true);
                        EnableTrackTagging(true);
                        EnableYearTagging(false);
                        break;
                    case "mkv":
                    case "ts":
                        EnableAlbumTagging(false);
                        EnableAlbumArtistTagging(false);
                        EnableArtistTagging(false);
                        EnableCommentTagging(false);
                        EnableDiscTagging(false);
                        EnableGenreTaggin(false);
                        EnablePublisherTagging(false);
                        EnableTitleTagging(true);
                        EnableTrackTagging(false);
                        EnableYearTagging(false);
                        break;
                    case "wmv":
                        EnableAlbumTagging(false);
                        EnableAlbumArtistTagging(false);
                        EnableArtistTagging(false);
                        EnableCommentTagging(true);
                        EnableDiscTagging(false);
                        EnableGenreTaggin(false);
                        EnablePublisherTagging(false);
                        EnableTitleTagging(true);
                        EnableTrackTagging(false);
                        EnableYearTagging(false);
                        break;
                    default:
                        EnableAlbumTagging(true);
                        EnableAlbumArtistTagging(true);
                        EnableArtistTagging(true);
                        EnableCommentTagging(true);
                        EnableDiscTagging(true);
                        EnableGenreTaggin(true);
                        EnablePublisherTagging(true);
                        EnableTitleTagging(true);
                        EnableTrackTagging(true);
                        EnableYearTagging(true);
                        break;
                }
            }
        }

        // Misc methods
        private void ExitFFmpegCatapult()
        {
            if (Session.SaveProperties == true)
            {
                Session.SaveSettings();
            }
            System.Environment.Exit(0);
        }

        private void EnableFileOutputControls(bool enable)
        {
            labelOutput.Enabled = enable;
            textBoxOutFile.Enabled = enable;
            buttonBrowseOutput.Enabled = enable;
            checkBoxOverwrite.Enabled = enable;
        }

        private void EnableRatioControls(bool enable)
        {
            labelRatio.Enabled = enable;
            textBoxRatioA.Enabled = enable;
            labelRatioDash.Enabled = enable;
            textBoxRatioB.Enabled = enable;
        }

        private void EnableLayoutControls(bool enable)
        {
            labelLayoutColour.Enabled = enable;
            labelLayoutHeight.Enabled = enable;
            labelLayoutWidth.Enabled = enable;
            labelHoriz.Enabled = enable;
            labelVert.Enabled = enable;
            textBoxLayoutColour.Enabled = enable;
            textBoxLayoutHeight.Enabled = enable;
            textBoxLayoutHoriz.Enabled = enable;
            textBoxLayoutVert.Enabled = enable;
            textBoxLayoutWidth.Enabled = enable;
        }

        private void EnableResControls(bool enable)
        {
            textBoxHeight.Enabled = enable;
            textBoxWidth.Enabled = enable;
        }

        private void EnableCRFControls(bool enable)
        {
            if (enable == true)
            {
                comboBoxBits.Enabled = false;
                labelVideoBitrate.Enabled = false;
                labelMaxBits.Enabled = false;
                labelMaxBitrate.Enabled = false;
                labelMinBits.Enabled = false;
                labelMinBitrate.Enabled = false;
                labelCRF.Enabled = true;
                textBoxVideoBitrate.Enabled = false;
                textBoxMaxBitrate.Enabled = false;
                textBoxMinBitrate.Enabled = false;
                textBoxCRF.Enabled = true;
            }
            else
            {
                comboBoxBits.Enabled = true;
                labelVideoBitrate.Enabled = true;
                labelMaxBits.Enabled = true;
                labelMaxBitrate.Enabled = true;
                labelMinBits.Enabled = true;
                labelMinBitrate.Enabled = true;
                labelCRF.Enabled = false;
                textBoxVideoBitrate.Enabled = true;
                textBoxMaxBitrate.Enabled = true;
                textBoxMinBitrate.Enabled = true;
                textBoxCRF.Enabled = false;
            }
        }

        private void EnableVideoControls(bool enable)
        {
            checkBoxTwoPassEncoding.Enabled = enable;
            groupBoxVideoBitrate.Enabled = enable;
            groupBoxVideoEncoder.Enabled = enable;
        }

        private void EnableAudioControls(bool enable)
        {
            if (enable == true && Audio.Codec == "copy" || Audio.Codec == "pcm")
            {
                groupBoxAudioBitrate.Enabled = false;
            }
            else
            {
                groupBoxAudioBitrate.Enabled = enable;
            }

            groupBoxAudioEncoder.Enabled = enable;
            groupBoxAudioOutput.Enabled = enable;
        }

        private void EnableTaggingControls(bool enable)
        {
            groupBoxGeneralTags.Enabled = enable;
            groupBoxTrackTags.Enabled = enable;
            groupBoxMiscTags.Enabled = enable;

            if (enable == false)
            {
                ClearMetadataFields();
            }
        }

        private void EnableAlbumTagging(bool enable)
        {
            labelAlbum.Enabled = enable;
            textBoxAlbum.Enabled = enable;

            if (enable == false)
            {
                textBoxAlbum.Text = "";
            }
        }

        private void EnableAlbumArtistTagging(bool enable)
        {
            labelAlbumArtist.Enabled = enable;
            textBoxAlbumArtist.Enabled = enable;

            if (enable == false)
            {
                textBoxAlbumArtist.Text = "";
            }
        }

        private void EnableArtistTagging(bool enable)
        {
            labelArtist.Enabled = enable;
            textBoxArtist.Enabled = enable;

            if (enable == false)
            {
                textBoxArtist.Text = "";
            }
        }

        private void EnableCommentTagging(bool enable)
        {
            labelComment.Enabled = enable;
            textBoxComment.Enabled = enable;

            if (enable == false)
            {
                textBoxComment.Text = "";
            }
        }

        private void EnableDiscTagging(bool enable)
        {
            labelDisc.Enabled = enable;
            labelOfDiscs.Enabled = enable;
            textBoxDisc.Enabled = enable;
            textBoxTotalDiscs.Enabled = enable;

            if (enable == false)
            {
                textBoxDisc.Text = "";
                textBoxTotalDiscs.Text = "";
            }
        }

        private void EnableGenreTaggin(bool enable)
        {
            labelGenre.Enabled = enable;
            textBoxGenre.Enabled = enable;

            if (enable == false)
            {
                textBoxGenre.Text = "";
            }
        }

        private void EnablePublisherTagging(bool enable)
        {
            labelPublisher.Enabled = enable;
            textBoxPublisher.Enabled = enable;

            if (enable == false)
            {
                textBoxPublisher.Text = "";
            }
        }

        private void EnableTitleTagging(bool enable)
        {
            labelTitle.Enabled = enable;
            textBoxTitle.Enabled = enable;

            if (enable == false)
            {
                textBoxTitle.Text = "";
            }
        }

        private void EnableTrackTagging(bool enable)
        {
            labelTrack.Enabled = enable;
            labelOfTrack.Enabled = enable;
            textBoxTrack.Enabled = enable;
            textBoxTotalTracks.Enabled = enable;

            if (enable == false)
            {
                textBoxTrack.Text = "";
                textBoxTotalTracks.Text = "";
            }
        }

        private void EnableYearTagging(bool enable)
        {
            labelYear.Enabled = enable;
            textBoxYear.Enabled = enable;

            if (enable == false)
            {
                textBoxYear.Text = "";
            }
        }

        private void EnableLogFileTextBox(bool enable)
        {            
            textBoxLog.Enabled = enable;
        }

        private void ClearMetadataFields()
        {
            textBoxAlbum.Text = "";
            textBoxAlbumArtist.Text = "";
            textBoxArtist.Text = "";
            textBoxComment.Text = "";
            textBoxDisc.Text = "";
            textBoxGenre.Text = "";
            textBoxPublisher.Text = "";
            textBoxTitle.Text = "";
            textBoxTotalDiscs.Text = "";
            textBoxTotalTracks.Text = "";
            textBoxTrack.Text = "";
            textBoxYear.Text = "";
        }

        private void EnableBinArgsControls(bool enable)
        {
            labelBinArgs.Enabled = enable;
            textBoxBinArgs.Enabled = enable;
            buttonBrowseFFmpegBin.Enabled = enable;
        }

        private void EnableTermArgsControls(bool enable)
        {
            labelTermArgs.Enabled = enable;
            textBoxTermArgs.Enabled = enable;
            buttonBrowseTermBin.Enabled = enable;
        }        

        //
        // Event handlers
        //

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitTabs();

            // Main tab
            if (File.Input != null)
            {
                textBoxInFile.Text = File.Input;
                buttonBrowseInput.Enabled = true;
            }
            textBoxInFile.DragDrop += new DragEventHandler(textBoxInFile_DragDrop);
            textBoxInFile.DragEnter += new DragEventHandler(textBoxInFile_DragEnter);
            textBoxInFile.TextChanged += new EventHandler(textBoxInFile_TextChanged);
            buttonBrowseInput.Click += new EventHandler(buttonBrowseInput_Click);

            if (File.Output != null)
            {
                textBoxOutFile.Text = File.Output;
            }
            textBoxOutFile.TextChanged += new EventHandler(textBoxOutFile_TextChanged);
            buttonBrowseOutput.Click += new EventHandler(buttonBrowseOutput_Click);

            for (int i = 0; i < File.Formats.GetLength(0); i++)
            {
                comboBoxContainers.Items.Add(new Methods.ListComboContent(File.Formats[i, 0], File.Formats[i, 1]));
            }

            comboBoxPresets.Items.Add(new Methods.ListComboContent("Default", null));

            // Populate combobox with parsed XML files and preset names
            string path = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(path, "*.xml");

            foreach (string file in files)
            {              
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlNodeList nodes = doc.SelectNodes("/presets/preset");

                    foreach (XmlNode node in nodes)
                    {
                        if (node.Attributes["name"] != null)
                        {
                            string title = node.Attributes["name"].Value;
                            comboBoxPresets.Items.Add(new Methods.ListComboContent(title, file));
                        }
                    }
                }
                catch (XmlException)
                {
                    MessageBox.Show("Invalid XML file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (IOException)
                {
                    MessageBox.Show("Unable to access file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }                
            }

            // Set selected preset
            int x = 0;
            for (int i = 0; i < comboBoxPresets.Items.Count; i++)
            {
                Methods.ListComboContent preset = (Methods.ListComboContent)comboBoxPresets.Items[i];
                if (preset.Name == Session.Preset)
                {
                    x = i;
                    break;
                }
            }
            comboBoxPresets.SelectedIndex = x;
            comboBoxPresets.SelectedIndexChanged += new EventHandler(comboBoxPresets_SelectedIndexChanged);

            comboBoxThreads.Items.Add(new Methods.ListComboContent("Auto", 0));
            for (int i = 1; i <= Session.MaxThreads; i++)
            {
                comboBoxThreads.Items.Add(new Methods.ListComboContent(Convert.ToString(i), i));
            }

            if (Session.KeepValues == true)
            {
                radioButtonKeep.Checked = true;
            }
            else
            {
                radioButtonRefresh.Checked = true;
            }
            radioButtonKeep.CheckedChanged += new EventHandler(radioButtonKeep_CheckedChanged);
            radioButtonRefresh.CheckedChanged += new EventHandler(radioButtonRefresh_CheckedChanged);

            InitMain();

            checkBoxOverwrite.Checked = Session.Overwrite;
            checkBoxOverwrite.CheckedChanged += new EventHandler(checkBoxOverwrite_CheckedChanged);

            this.FormClosing += MainForm_Closing;
            buttonExit.Click += new EventHandler(buttonExit_Click);
            buttonRun.Click += new EventHandler(buttonRun_Click);

            // Streams
            textBoxAudioStream.TextChanged += new EventHandler(textBoxAudioStream_TextChanged);
            buttonBrowseAudioStream.Click += new EventHandler(buttonBrowseAudioStream_Click);

            // Picture tab
            for (int i = 0; i < Screen.ScalingMethods.GetLength(0); i++)
            {
                comboBoxScalingMethods.Items.Add(new Methods.ListComboContent(Screen.ScalingMethods[i, 0], Screen.ScalingMethods[i, 1]));
            }

            InitPicture();

            // Video tab
            comboBoxBits.Items.Add(new Methods.ListComboContent("Kbps", "k"));
            comboBoxBits.Items.Add(new Methods.ListComboContent("Mbps", "M"));
            comboBoxBytes.Items.Add(new Methods.ListComboContent("KB", "k"));
            comboBoxBytes.Items.Add(new Methods.ListComboContent("MB", "M"));
            comboBoxBytes.Items.Add(new Methods.ListComboContent("GB", "G"));

            buttonVideoCodecProperties.Click += new EventHandler(buttonVideoCodecProperties_Clicked);

            InitVideo();

            // Audio tab
            buttonAudioCodecProperties.Click += new EventHandler(buttonAudioCodecProperties_Clicked);

            InitAudio();

            // Options           
            if (Bin.FFmpegBin != null)
            {
                textBoxFFmpegBin.Text = Bin.FFmpegBin;
                EnableBinArgsControls(true);
                EnableTermArgsControls(true);                
            }
            else
            {
                EnableBinArgsControls(false);
                EnableTermArgsControls(false); 
            }
            textBoxFFmpegBin.TextChanged += new EventHandler(textBoxFFmpegBin_TextChanged);
            buttonBrowseFFmpegBin.Click += new EventHandler(buttonBrowseFFmpegBin_Click);

            if (Bin.TermBin != null)
            {
                textBoxTermBin.Text = Bin.TermBin;
                EnableTermArgsControls(true);
            }
            else
            {
                EnableTermArgsControls(false);
            }
            textBoxTermBin.TextChanged += new EventHandler(textBoxTermBin_TextChanged);
            buttonBrowseTermBin.Click += new EventHandler(buttonBrowseTermBin_Click);

            if (Bin.BinArgs != null)
            {
                textBoxBinArgs.Text = Bin.BinArgs;
            }
            textBoxBinArgs.TextChanged += new EventHandler(textBoxBinArgs_TextChanged);

            if (Bin.TermArgs != null)
            {
                textBoxTermArgs.Text = Bin.TermArgs;
            }
            textBoxTermArgs.TextChanged += new EventHandler(textBoxTermArgs_TextChanged);

            checkBoxWriteLog.Checked = Session.WriteLog;
            checkBoxWriteLog.CheckedChanged += new EventHandler(checkBoxWriteLog_CheckedChanged);
            EnableLogFileTextBox(Session.WriteLog);
            textBoxLog.Text = File.Log;
            textBoxLog.TextChanged += new EventHandler(textBoxLog_TextChanged);
            checkBoxSaveSettings.Checked = Session.SaveProperties;
            checkBoxSaveSettings.CheckedChanged += new EventHandler(checkBoxSaveSettings_CheckedChanged);

            // Metadata tab
            textBoxAlbum.TextChanged += new EventHandler(textBoxAlbum_TextChanged);
            textBoxAlbumArtist.TextChanged += new EventHandler(textBoxAlbumArtist_TextChanged);
            textBoxArtist.TextChanged += new EventHandler(textBoxArtist_TextChanged);
            textBoxComment.TextChanged += new EventHandler(textBoxComment_TextChanged);
            textBoxDisc.TextChanged += new EventHandler(textBoxDisc_TextChanged);
            textBoxGenre.TextChanged += new EventHandler(textBoxGenre_TextChanged);
            textBoxTitle.TextChanged += new EventHandler(textBoxTitle_TextChanged);
            textBoxTotalDiscs.TextChanged += new EventHandler(textBoxTotalDiscs_TextChanged);
            textBoxTotalTracks.TextChanged += new EventHandler(textBoxTotalTracks_TextChanged);
            textBoxTrack.TextChanged += new EventHandler(textBoxTrack_TextChanged);
            textBoxTrack.TextChanged += new EventHandler(textBoxTrack_TextChanged);
            textBoxYear.TextChanged += new EventHandler(textBoxYear_TextChanged);

            InitMetadata();
        }

        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            ExitFFmpegCatapult();
        }

        void buttonExit_Click(object sender, EventArgs e)
        {
            ExitFFmpegCatapult();
        }

        void buttonRun_Click(object sender, EventArgs e)
        {
            Bin.Run();

            if (Session.KeepValues == false)
            {
                textBoxInFile.Text = "";
                textBoxOutFile.Text = "";
                ClearMetadataFields();
            }
        }

        // Key press event handler for numbered values
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void textBoxBitrate_KeyPressDecimal(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
            {
                if (e.KeyChar != '.' | Video.Bits == "k")
                {
                    e.Handled = e.KeyChar != (char)Keys.Back;
                }
            }
        }

        private void textBoxBufferSize_KeyPressDecimal(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
            {
                if (e.KeyChar != '.' | Video.Bytes == "k")
                {
                    e.Handled = e.KeyChar != (char)Keys.Back;
                }
            }
        }

        // Main tab event handlers
        private void textBoxInFile_TextChanged(object sender, EventArgs e)
        {
            File.Input = textBoxInFile.Text;

            if (!string.IsNullOrEmpty(textBoxInFile.Text))
            {
                EnableFileOutputControls(true);
            }
            else
            {
                EnableFileOutputControls(false);
                textBoxOutFile.Text = "";
            }

            if (!string.IsNullOrEmpty(textBoxInFile.Text) && string.IsNullOrEmpty(textBoxOutFile.Text))
            {
                buttonRun.Enabled = true;
            }
            else
            {
                buttonRun.Enabled = false;
            }
        }

        private void textBoxOutFile_TextChanged(object sender, EventArgs e)
        {
            if (textBoxOutFile.Text != "")
            {
                File.Output = textBoxOutFile.Text;
                textBoxOutFile.Text = File.Output;
            }
            else
            {
                File.Output = "";
            }

            if (textBoxInFile.Text != "" && textBoxOutFile.Text != "")
            {
                buttonRun.Enabled = true;
            }
            else
            {
                buttonRun.Enabled = false;
            }
        }

        private void buttonBrowseInput_Click(object sender, EventArgs e)
        {
            OpenFileDialog inFile = new OpenFileDialog();
            inFile.ShowDialog();
            inFile.Filter = "Any file (*.*) | *.*";

            if (inFile.FileName != "")
            {                
                textBoxInFile.Text = inFile.FileName;
                textBoxOutFile.Text = inFile.FileName;
            }            
        }

        private void buttonBrowseOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog outFile = new SaveFileDialog();
            if (fileExtension == "custom")
            {
                outFile.Filter = "Any file *.* | *.*";
            }
            else
            {
                outFile.Filter = String.Format("{0} (*.{1}) | *.{1}", fileContainer, fileExtension);
            }
            outFile.ShowDialog();

            if (outFile.FileName != "")
            {
                textBoxOutFile.Text = outFile.FileName;
            }
        }

        void textBoxInFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void textBoxInFile_DragDrop(object sender, DragEventArgs e)
        {
            String[] file = (String[])e.Data.GetData(DataFormats.FileDrop);
            if (file != null && file.Length != 0)
            {
                textBoxInFile.Text = file[0];
                textBoxOutFile.Text = file[0];
            }
        }

        void checkBoxOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            Session.Overwrite = checkBoxOverwrite.Checked;
        }

        void checkBoxMultiThreading_CheckedChanged(object sender, EventArgs e)
        {
            Session.MultiThreading = checkBoxMultiThreading.Checked;
            comboBoxThreads.Enabled = Session.MultiThreading;
        }

        void comboBoxContainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent format = (Methods.ListComboContent)comboBoxContainers.SelectedItem;

            if (format.Value != File.Format)
            {
                File.Format = format.Value;
                fileContainer = format.Name;
                fileExtension = format.Value;

                if (textBoxOutFile.Text != "" && File.Format != "custom")
                {
                    textBoxOutFile.TextChanged -= new EventHandler(textBoxOutFile_TextChanged);
                    File.Output = textBoxOutFile.Text;
                    textBoxOutFile.Text = File.Output;
                    textBoxOutFile.TextChanged += new EventHandler(textBoxOutFile_TextChanged);
                }
            }

            InitTabs();
            InitAudio();
            InitVideo();
            InitMetadata();
        }

        void comboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent preset = (Methods.ListComboContent)comboBoxPresets.SelectedItem;
            Session.Preset = preset.Name;
            Preset.InitPreset(preset.Name, preset.Value);
            InitTabs();
            InitMain();
            InitPicture();
            InitVideo();
            InitAudio();
            InitMetadata();
        }

        void comboBoxThreads_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent threads = (Methods.ListComboContent)comboBoxThreads.SelectedItem;
            Session.Threads = threads.X;
        }

        void radioButtonKeep_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonKeep.Checked == true)
            {
                Session.KeepValues = true;
            }
        }

        void radioButtonRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRefresh.Checked == true)
            {
                Session.KeepValues = false;
            }
        }

        // Picture tab events
        void radioButtonKeepRes_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonKeepRes.Checked == true)
            {
                Screen.ScaleOption = 0;
                EnableResControls(false);
            }
        }

        void radioButtonCustomRes_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCustomRes.Checked == true)
            {
                Screen.ScaleOption = 1;
                EnableResControls(true);
            }
        }

        void radioButtonHalfRes_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHalfRes.Checked == true)
            {
                Screen.ScaleOption = 2;
                EnableResControls(false);
            }
        }

        void checkBoxAspectRatio_CheckedChanged(object sender, EventArgs e)
        {
            Screen.AspectRatio = checkBoxAspectRatio.Checked;
            EnableRatioControls(Screen.AspectRatio);
        }

        void checkBoxCrop_CheckedChanged(object sender, EventArgs e)
        {
            Screen.CropVideo = checkBoxCrop.Checked;
            EnableLayoutControls(Screen.CropVideo);
        }

        void checkBoxDeinterlace_CheckedChanged(object sender, EventArgs e)
        {
            Screen.Deinterlace = checkBoxDeinterlace.Checked;
        }

        void checkBoxPad_CheckedChanged(object sender, EventArgs e)
        {
            Screen.PadVideo = checkBoxPad.Checked;
            EnableLayoutControls(Screen.PadVideo);
        }

        void textBoxWidth_TextChanged(object sender, EventArgs e)
        {
            Screen.Width = Methods.TextToInt(textBoxWidth.Text);
        }

        void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            Screen.Height = Methods.TextToInt(textBoxHeight.Text);
        }

        void textBoxRatioA_TextChanged(object sender, EventArgs e)
        {
            Screen.RatioA = Methods.TextToInt(textBoxRatioA.Text);
        }

        void textBoxRatioB_TextChanged(object sender, EventArgs e)
        {
            Screen.RatioB = Methods.TextToInt(textBoxRatioB.Text);
        }

        void textBoxFPS_TextChanged(object sender, EventArgs e)
        {
            Screen.FPS = Methods.TextToInt(textBoxFPS.Text);
        }

        void textBoxLayoutWidth_TextChanged(object sender, EventArgs e)
        {
            Screen.WinWidth = Methods.TextToInt(textBoxLayoutWidth.Text);
        }

        void textBoxLayoutHeight_TextChanged(object sender, EventArgs e)
        {
            Screen.WinHeight = Methods.TextToInt(textBoxLayoutHeight.Text);
        }

        void textBoxLayoutVert_TextChanged(object sender, EventArgs e)
        {
            Screen.X = Methods.TextToInt(textBoxLayoutVert.Text);
        }

        void textBoxLayoutHoriz_TextChanged(object sender, EventArgs e)
        {
            Screen.Y = Methods.TextToInt(textBoxLayoutHoriz.Text);
        }

        void comboBoxScalingMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent scalingMethod = (Methods.ListComboContent)comboBoxScalingMethods.SelectedItem;
            Screen.ScalingMethod = scalingMethod.Value;
        }

        // Video tab events
        void buttonVideoCodecProperties_Clicked(object sender, EventArgs e)
        {
            VideoSettingsForm videoProperties = new VideoSettingsForm();
            videoProperties.StartPosition = FormStartPosition.CenterParent;
            videoProperties.ShowDialog(this);
        }

        void comboBoxVideoCodecs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent codec = (Methods.ListComboContent)comboBoxVideoCodecs.SelectedItem;
            if (codec.Value != Video.Codec)
            {
                Video.Codec = codec.Value;
                InitTabs();
                InitVideo();
            }
        }

        void comboBoxVideoEncoders_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent encoder = (Methods.ListComboContent)comboBoxVideoEncoders.SelectedItem;
            Video.Encoder = encoder.Value;
        }

        void comboBoxBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent bits = (Methods.ListComboContent)comboBoxBits.SelectedItem;
            Video.Bits = bits.Value;
            labelMaxBits.Text = bits.Name;
            labelMinBits.Text = bits.Name;
        }

        void comboBoxBytes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent bytes = (Methods.ListComboContent)comboBoxBytes.SelectedItem;
            Video.Bytes = bytes.Value;
        }

        void checkBoxTwoPassEncoding_CheckedChanged(object sender, EventArgs e)
        {
            Session.TwoPassEncoding = checkBoxTwoPassEncoding.Checked;
            if (Session.TwoPassEncoding)
            {
                checkBoxUseCRF.Enabled = false;
                EnableCRFControls(false);
            }
            else
            {
                checkBoxUseCRF.Enabled = true;
                EnableCRFControls(Video.UseCRF);
            }
        }

        void checkBoxUseCRF_CheckedChanged(object sender, EventArgs e)
        {
            Video.UseCRF = checkBoxUseCRF.Checked;
            EnableCRFControls(Video.UseCRF);
        }

        void textBoxVideoBitrate_TextChanged(object sender, EventArgs e)
        {
            Video.Bitrate = Methods.TextToDouble(textBoxVideoBitrate.Text);
        }

        void textBoxMinBitrate_TextChanged(object sender, EventArgs e)
        {
            Video.MinBitrate = Methods.TextToDouble(textBoxMinBitrate.Text);
        }

        void textBoxMaxBitrate_TextChanged(object sender, EventArgs e)
        {
            Video.MaxBitrate = Methods.TextToDouble(textBoxMaxBitrate.Text);
        }

        void textBoxBufferSize_TextChanged(object sender, EventArgs e)
        {
            Video.BufferSize = Methods.TextToDouble(textBoxBufferSize.Text);
        }

        void textBoxCRF_TextChanged(object sender, EventArgs e)
        {
            Video.CRF = Methods.TextToInt(textBoxCRF.Text);
        }

        void textBoxQmax_TextChanged(object sender, EventArgs e)
        {
            Video.Qmax = Methods.TextToInt(textBoxQmax.Text);
        }

        void textBoxQmin_TextChanged(object sender, EventArgs e)
        {
            Video.Qmin = Methods.TextToInt(textBoxQmin.Text);
        }

        // Audio tab events
        void buttonAudioCodecProperties_Clicked(object sender, EventArgs e)
        {
            AudioSettingsForm audioProperties = new AudioSettingsForm();
            audioProperties.StartPosition = FormStartPosition.CenterParent;
            audioProperties.ShowDialog(this);
        }

        void comboBoxAudioCodecs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent codec = (Methods.ListComboContent)comboBoxAudioCodecs.SelectedItem;
            if (codec.Value != Audio.Codec)
            {
                Audio.Codec = codec.Value;
                InitAudio();
            }
        }

        void comboBoxAudioEncoders_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent encoder = (Methods.ListComboContent)comboBoxAudioEncoders.SelectedItem;
            if (encoder.Value != Audio.Encoder)
            {
                Audio.Encoder = encoder.Value;
                InitAudioEncPropertiesButton();
                InitAudioBitrates();
            }
        }

        void comboBoxSampleRates_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent sampleRate = (Methods.ListComboContent)comboBoxSampleRates.SelectedItem;
            if (sampleRate.X != Audio.SampleRate)
            {
                Audio.SampleRate = sampleRate.X;
                InitAudioBitrates();
            }
        }

        void comboBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent channels = (Methods.ListComboContent)comboBoxChannels.SelectedItem;
            if (channels.X != Audio.Channels)
            {
                Audio.Channels = channels.X;
            }
        }

        void checkBoxUseAudioVBR_CheckedChanged(object sender, EventArgs e)
        {
            Audio.UseVBR = checkBoxUseAudioVBR.Checked;
            InitAudioBitrates();
        }

        void comboBoxAudioBitrates_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.ListComboContent bitrate = (Methods.ListComboContent)comboBoxAudioBitrates.SelectedItem;

            if (Audio.UseVBR == true)
            {
                Audio.Quality = bitrate.X;
            }
            else
            {
                Audio.Bitrate = bitrate.X;
            }
        }

        void buttonBrowseAudioStream_Click(object sender, EventArgs e)
        {
            OpenFileDialog audioFile = new OpenFileDialog();
            audioFile.ShowDialog();

            if (audioFile.FileName != "")
            {
                textBoxAudioStream.Text = audioFile.FileName;
            }
        }

        void textBoxAudioStream_TextChanged(object sender, EventArgs e)
        {
            File.AudioStream = textBoxAudioStream.Text;
        }

        // Tagging tab events
        void textBoxAlbum_TextChanged(object sender, EventArgs e)
        {
            Metadata.Album = textBoxAlbum.Text;
        }

        void textBoxAlbumArtist_TextChanged(object sender, EventArgs e)
        {
            Metadata.AlbumArtist = textBoxAlbumArtist.Text;
        }

        void textBoxArtist_TextChanged(object sender, EventArgs e)
        {
            Metadata.Artist = textBoxArtist.Text;
        }

        void textBoxComment_TextChanged(object sender, EventArgs e)
        {
            Metadata.Comment = textBoxComment.Text;
        }

        void textBoxDisc_TextChanged(object sender, EventArgs e)
        {
            Metadata.Disc = Methods.TextToInt(textBoxDisc.Text);
        }

        void textBoxGenre_TextChanged(object sender, EventArgs e)
        {
            Metadata.Genre = textBoxGenre.Text;
        }

        void textBoxTitle_TextChanged(object sender, EventArgs e)
        {
            Metadata.Title = textBoxTitle.Text;
        }

        void textBoxTotalDiscs_TextChanged(object sender, EventArgs e)
        {
            Metadata.Disc = Methods.TextToInt(textBoxDisc.Text);
        }

        void textBoxTotalTracks_TextChanged(object sender, EventArgs e)
        {
            Metadata.TotalTracks = Methods.TextToInt(textBoxTotalTracks.Text);
        }

        void textBoxTrack_TextChanged(object sender, EventArgs e)
        {
            Metadata.Track = Methods.TextToInt(textBoxTrack.Text);
        }

        void textBoxYear_TextChanged(object sender, EventArgs e)
        {
            Metadata.Year = Methods.TextToInt(textBoxYear.Text);
        }

        // Options tab events
        void textBoxFFmpegBin_TextChanged(object sender, EventArgs e)
        {
            Bin.FFmpegBin = textBoxFFmpegBin.Text;
            EnableBinArgsControls(!string.IsNullOrEmpty(textBoxFFmpegBin.Text));
        }

        void textBoxTermBin_TextChanged(object sender, EventArgs e)
        {
            Bin.TermBin = textBoxTermBin.Text;
            EnableTermArgsControls(!string.IsNullOrEmpty(textBoxTermBin.Text));
        }

        void textBoxBinArgs_TextChanged(object sender, EventArgs e)
        {
            Bin.BinArgs = textBoxBinArgs.Text;
        }

        void textBoxTermArgs_TextChanged(object sender, EventArgs e)
        {
            Bin.TermArgs = textBoxTermArgs.Text;
        }

        void textBoxLog_TextChanged(object sender, EventArgs e)
        {
            File.Log = textBoxLog.Text;
        }

        void buttonBrowseFFmpegBin_Click(object sender, EventArgs e)
        {
            OpenFileDialog binFile = new OpenFileDialog();
            binFile.Filter = "Executable (*.exe) | *.exe | Any file (*.*) | *.*";
            binFile.ShowDialog();

            if (binFile.FileName != "")
            {
                textBoxFFmpegBin.Text = binFile.FileName;
            }
        }

        void checkBoxWriteLog_CheckedChanged(object sender, EventArgs e)
        {
            Session.WriteLog = checkBoxWriteLog.Checked;
            EnableLogFileTextBox(Session.WriteLog);
        }

        void checkBoxSaveSettings_CheckedChanged(object sender, EventArgs e)
        {
            Session.SaveProperties = checkBoxSaveSettings.Checked;
            Properties.Settings.Default.SaveSettings = Session.SaveProperties;
            Properties.Settings.Default.Save();
        }

        void buttonBrowseTermBin_Click(object sender, EventArgs e)
        {
            OpenFileDialog termBinFile = new OpenFileDialog();
            termBinFile.Filter = "Executable (*.exe) | *.exe | Any file (*.*) | *.*";
            termBinFile.ShowDialog();

            if (termBinFile.FileName != "")
            {
                textBoxTermBin.Text = termBinFile.FileName;
            }
        }
    }
}