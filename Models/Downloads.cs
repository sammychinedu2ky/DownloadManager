﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;

namespace DownloadManager.Models
{
    public partial class Downloads : INotifyPropertyChanged
    {
        private string _FileId;
        public string FileId
        {
            get => _FileId;
            set
            {
                _FileId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileId)));
            }
        }
        private string _FilePath;
        public string FilePath
        {
            get => _FilePath;
            set
            {
                _FilePath = value;
                FileName = new FileInfo(FilePath).Name;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileName)));


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilePath)));
            }
        }
        private long _OnlineSize;
        public long OnlineSize
        {
            get => _OnlineSize;
            set
            {
                _OnlineSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OnlineSize)));
            }
        }

        private string _Url;
        public string Url
        {
            get => _Url; set
            {
                _Url = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Url)));
            }
        }
        private string _FileName;
        
        [NotMapped]
        public string FileName
        {
            get => _FileName;
            set
            {
                _FileName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileName)));
            }
        }


        private bool _IsComplete;
        
        [NotMapped]
        public bool IsComplete
        {
            get => _IsComplete;
            set
            {
               // if
                _IsComplete = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsComplete)));
            }
        }

        private long _LocalSize;

        [NotMapped]
        public long LocalSize
        {
            get => _LocalSize;
            set
            {
                // if
                _LocalSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LocalSize)));
            }
        }

        private double _Progress ;

        [NotMapped]
        public double Progress
        {
            get => _Progress;
            set
            {
              
                _Progress = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
            }
        }

        private bool _Pause;
        [NotMapped]
        public bool Pause
        {
            get => _Pause;
            set
            {
                // if
                _Pause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pause)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}