using NPoco;

namespace BoschVG1.Models
{
    [TableName("PalletMatrix")]
    public class PalletMatrix
    {
        [Column("FG Part Numb")]
        [Models.InExcelSheet.Column("FG Part Numb")]
        public string FG_Part_Number { get; set; }
        [Column("FG Part Num")]
        [Models.InExcelSheet.Column("FG Part Num")]
        public string FG_Part_Numb { get; set; }
        [Column("land")]
        [Models.InExcelSheet.Column("land")]
        public string land { get; set; }
        [Column("Family")]
        [Models.InExcelSheet.Column("Family")]
        public string Family { get; set; }
        [Column("Box/Bin Qty")]
        [Models.InExcelSheet.Column("Box/Bin Qty")]
        public string Box_Bin_Qty { get; set; }
        [Column("Pallet Qty")]
        [Models.InExcelSheet.Column("Pallet Qty")]
        public string Pallet_Quantity { get; set; }
        [Column("Corrugated Box Used")]
        [Models.InExcelSheet.Column("Corrugated Box Used")]
        public string Pallet_Qty { get; set; }
        [Column("Pallet No (1 unit)")]
        [Models.InExcelSheet.Column("Pallet No (1 unit)")]
        public string Corrugated_Box_Used { get; set; }
        [Column("cfg_key")]
        [Models.InExcelSheet.Column("")]
        public string Pallet_No_1_unit { get; set; }
        [Column("Top/bottom cover (2UNIT)")]
        [Models.InExcelSheet.Column("Top/bottom cover (2UNIT)")]
        public string Top_bottom_cover_2UNIT { get; set; }
        [Column("Corner guard (8)")]
        [Models.InExcelSheet.Column("Corner guard (8)")]
        public string Corner_guard_8 { get; set; }
        [Column("Plastic edge protector")]
        [Models.InExcelSheet.Column("Plastic edge protector")]
        public string Plastic_edge_protector { get; set; }
        [Column("Side wall(1)")]
        [Models.InExcelSheet.Column("Side wall(1)")]
        public string Side_wall_1 { get; set; }
        [Column("Pallet cover")]
        [Models.InExcelSheet.Column("Pallet cover")]
        public string Pallet_cover { get; set; }
        [Column("Strapping")]
        [Models.InExcelSheet.Column("Strapping")]
        public string Strapping { get; set; }
        [Column("Stretch Wrapping")]
        [Models.InExcelSheet.Column("Stretch Wrapping")]
        public string Stretch_Wrapping { get; set; }
        [Column("VCI")]
        [Models.InExcelSheet.Column("VCI")]
        public string VCI { get; set; }
        [Column("Tape")]
        [Models.InExcelSheet.Column("Tape")]
        public string Tape { get; set; }

    }
}
