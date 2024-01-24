﻿using Application.Common.MediatR;

namespace Application.Exceptions
{
    public class BadRequestException: AppException
    {
        public BadRequestException(string message):base(message)
        {

        }
    }
}
