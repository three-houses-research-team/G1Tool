﻿using G1TConverter.IO;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G1TConverter.Formats
{
    public class DDS
    {
        public uint Size { get; private set; }
        /// <summary>
        /// Width (in pixel)
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Height (in pixel)
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// Depth of a volume texture (in pixel)
        /// </summary>
        public uint Depth { get; private set; }
        /// <summary>
        /// Number of mipmap levels
        /// </summary>
        public uint MipMapCount { get; private set; }
        public uint PixelFormat { get; private set; }

        public uint Caps { get; private set; }

        public Texture2D Texture { get; private set; }

        public enum DDSCAPS : uint
        {
            COMPLEX = 0x00000008,
            TEXTURE = 0x00001000,
            MIPMAP = 0x00400000
        }

        public DDS()
        {
            Texture = new Texture2D();
        }

        public void Read(string filename)
        {
            Read(new EndianBinaryReader(filename, Endianness.Little));
        }

        public void Read(EndianBinaryReader r)
        {
            if (r.ReadUInt32() != 0x20534444)
            {
                MessageBox.Show("This file is not a DirectDraw Surface.", "Invalid file");
                return;
            }

            Size = r.ReadUInt32();
            uint flags = r.ReadUInt32();

            Height = r.ReadInt32();
            Width = r.ReadInt32();
            uint pitch = r.ReadUInt32();
            Depth = r.ReadUInt32();
            MipMapCount = r.ReadUInt32();
            r.SeekCurrent(4 * 11); //Skip reserved
            uint[] pixelformat = r.ReadUInt32s(8);
            Caps = r.ReadUInt32();
            uint burnes = r.ReadUInt32();
            uint[] unused = r.ReadUInt32s(3);

            Texture.LoadImageData(Width, Height, r.ReadBytes((int)(Width * Height / 2)), InternalFormat.CompressedRgbS3tcDxt1Ext);
        }
    }
}