namespace Bai6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sach")]
    public partial class Sach
    {
        [Key]
        [StringLength(10)]
        public string MaSach { get; set; }

        [Required]
        [StringLength(50)]
        public string TenSach { get; set; }

        public int NamXB { get; set; }

        public int MaLoai { get; set; }

        public virtual LoaiSach LoaiSach { get; set; }
    }
}
