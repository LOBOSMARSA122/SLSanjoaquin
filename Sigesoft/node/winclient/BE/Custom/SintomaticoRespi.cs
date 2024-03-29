﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class SintomaticoRespi
    {
        public string Trabajador { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public DateTime? FechaEvaluacion { get; set; }
        public string Puesto { get; set; }
        public string ServicioId { get; set; }
        public byte[] FirmaUsuarioGraba { get; set; }
        public string NombreUsuarioGraba { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public byte[] FirmaTrabajador { get; set; }
        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }

        public string SINTOMATICO_1 { get; set; }
        public string SINTOMATICO_2 { get; set; }
        public string SINTOMATICO_3 { get; set; }
        public string SINTOMATICO_4 { get; set; }
        public string SINTOMATICO_5 { get; set; }
        public string SINTOMATICO_6 { get; set; }
        public string SINTOMATICO_7 { get; set; }
        public string SINTOMATICO_OBS { get; set; }
        public string SINTOMATICO_SI_NO { get; set; }
        public string SINTOMATICO_BK_1 { get; set; }
        public string SINTOMATICO_BK_2 { get; set; }
        public string SINTOMATICO_RX { get; set; }
        public string TipoEso { get; set; }
        public string Dni { get; set; }
    }
}
