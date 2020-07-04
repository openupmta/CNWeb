namespace TriThucOnline_TTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KHUYENMAI")]
    public partial class KHUYENMAI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHUYENMAI()
        {
            DONHANGs = new HashSet<DONHANG>();
        }

        [Key]
        [StringLength(10)]
        public string MaKM { get; set; }

        public double? GiaTriKM { get; set; }

        public DateTime? NgayBD { get; set; }

        public DateTime? NgayKT { get; set; }

        public int? SoLanConLai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONHANG> DONHANGs { get; set; }
    }
}
