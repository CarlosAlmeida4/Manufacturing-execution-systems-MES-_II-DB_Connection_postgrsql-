using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;
using Npgsql;
using MES_NOVO;

// as bibliotecas extra usadas aqui estão disponiveis em
// easymodbus : https://sourceforge.net/projects/easymodbustcp/
//Npgsql : http://pgfoundry.org/frs/download.php/1964/Npgsql2.0.1-bin-ms.net3.5sp1.zip
// como adicionar ao projecto : https://msdn.microsoft.com/en-us/library/wkze6zky.aspx

namespace MES_NOVO
{
    // Nesta configuração o programa liga-se à fabrica
    // é preciso mudar register para coil (inteiro para bool)

    class Modbus
    {
        public string addr_client = "172.30.23.33"; //IP do servidor (maquina onde esta o isagraf)
        public int read_port = 5503;// numero da porta onde o isagraf escreve (refer to http://easymodbustcp.net/modbusclient-methods)
        public int write_port = 5504; // numero da porta onde o isagraf le (onde o MES escreve)

        //structs do modbus para fazer as comms
        ModbusClient client_read;
        ModbusClient client_write;



        // ao chamar esta função é necessario dar-lhe qual o primeiro registo a ler e qual a quantidade de registos
        // le informação guardada nos registos do isagraf
        // (refer to http://easymodbustcp.net/modbusclient-methods)
        // modo para passar o int para uma string
        //

        public int[] modbus_read(int startaddress, int quantity)
        {
            client_read = new ModbusClient(addr_client, read_port);
            client_read.Connect();

            // função que le os registos especificados
            int[] info = client_read.ReadHoldingRegisters(startaddress, quantity);

            client_read.Disconnect();
            return info;
        }

        // entra o start addr  e a infomação que queremos escrever para o isagraf
        // é possivel usar a funcao WriteMultipleRegister mas fazer tal coisa seria overthinking de momento
        // a funcao WriteSingleRegister n retorna nada, mas para facilitar o uso de modbus_read e modbus_write estas serão as duas int
        // modbus_write irá enviar 1 caso algo corra mal na escrita

        public int modbus_write(int startaddress, int info)
        {
            client_write = new ModbusClient(addr_client, write_port);
            client_write.Connect();
            

            client_write.WriteSingleRegister(startaddress, info);

            client_write.Disconnect();
            return 1;
        }
    }
}
        //Tutorial para instalar a biblioteca usada e como as funções sao utilizadas está aqui 
        //(https://www.codeproject.com/articles/30989/using-postgresql-in-your-c-net-application-an-intr)

