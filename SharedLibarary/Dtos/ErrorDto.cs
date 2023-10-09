﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibarary.Dtos
{
    public class ErrorDto
    {
        public List<String> Errors { get; private set; }
        //kullanıcılara gösterilecek olan hatalar false
        public bool IsShow { get; private set; }
        public ErrorDto()
        {
            Errors = new List<string>();

        }
        public ErrorDto(string error, bool isShow)
        {
            Errors.Add(error);
            isShow = true;
        }

        public ErrorDto(List<string> errors, bool ısShow)
        {
            Errors = errors;
            IsShow = ısShow;
        }
    }
}