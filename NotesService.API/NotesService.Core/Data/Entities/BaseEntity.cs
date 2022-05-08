﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesService.Core.Data.Entities
{
    public class BaseEntity<TId>
    {
        public TId Id { get; set; }
    }
}
