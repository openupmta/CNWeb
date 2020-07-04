namespace TriThucOnline_TTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DONHANG")]
    public partial class DONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONHANG()
        {
            CT_DONHANG = new HashSet<CT_DONHANG>();
        }

        [Key]
        public int MaDH { get; set; }

        public DateTime? NgayMuaHang { get; set; }

        public DateTime? NgayGiao { get; set; }

        [StringLength(50)]
        public string TrangThaiThanhToan { get; set; }

        public int? TrangThaiGiao { get; set; }

        public int? MaKH { get; set; }

        [StringLength(10)]
        public string MaKM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_DONHANG> CT_DONHANG { get; set; }

        public virtual KHACHHANG KHACHHANG { get; set; }

        public virtual KHUYENMAI KHUYENMAI { get; set; }
    }
}
