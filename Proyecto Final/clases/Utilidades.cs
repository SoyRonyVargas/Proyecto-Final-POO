namespace Proyecto_Final.clases
{
    public class Utilidades
    {
        public static string renderDinero( float monto )
        {

            string _monto = monto.ToString("0.00");

            return $"${_monto}";

        }
        public static string renderTipoPedido( int status )
        {

            if( status == 0 )
            {
                return "Comer Aqui";
            }
            
            if( status == 1 )
            {
                return "Para llevar";
            }

            return "Sin tipo";

        }
        public static string renderStatusPedido( int status )
        {

            if( status == 0 )
            {
                return "Pendiente";
            }
            
            if( status == 1 )
            {
                return "Terminado/Cobrado";
            }

            return "Cancelado";

        }
        public static int getTipoCobro( string tipo_cobro )
        {

            if( tipo_cobro == "Efectivo" )
            {
                return 0;
            }
            
            return 1;

        }
        
        public static string renderTipoCobro( int? tipo_cobro )
        {

            if( tipo_cobro == 0 )
            {
                return "Efectivo";
            }
            
            return "Tarjeta";

        }
        
    }
}

