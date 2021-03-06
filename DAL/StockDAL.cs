﻿using System.Collections.Generic;
using bidCardCoin.DAO;
using Npgsql;

namespace bidCardCoin.DAL
{
    public static class StockDAL
    {
        // SELECT
        public static StockDAO SelectStockById(string id)
        {
            var stockDao = new StockDAO();
            // Selectionne la stock a partir de l'id
            var query =
                "SELECT * FROM public.stock a where a.\"idStock\"=:idStockParam";
            var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
            cmd.Parameters.AddWithValue("idStockParam", id);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // récup les paramètres principaux
                var idStock = (string) reader["idStock"];
                var adresseId = (string) reader["adresseId"];

                stockDao = new StockDAO(idStock, adresseId);
            }

            reader.Close();
            return stockDao;
        }

        public static List<StockDAO> SelectAllStock()
        {
            // Selectionné tout les stock dans la base de donnée
            var liste = new List<StockDAO>();

            var query = "SELECT * FROM public.stock ORDER BY \"idStock\"";
            var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var idStock = (string) reader["idStock"];
                var adresseId = (string) reader["adresseId"];

                liste.Add(new StockDAO(idStock, adresseId));
            }

            reader.Close();
            return liste;
        }

        // INSERT & Update 
        public static void InsertOrAddNewStock(StockDAO stock)
        {
            // Inserer stock dans la bdd
            var query =
                @"INSERT INTO public.stock (""idStock"",""adresseId"") 
values (:idStock,:adresseId) 
ON CONFLICT ON CONSTRAINT pk_stock DO UPDATE SET ""idStock""=:idStock,
""adresseId""=:adresseId,
where stock.""idStock""=:idStock";
            var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
            cmd.Parameters.AddWithValue("idStock", stock.IdStock);
            cmd.Parameters.AddWithValue("adresseId", stock.AdresseId);

            cmd.ExecuteNonQuery();
        }

        // DELETE
        public static void DeleteStock(string stockId)
        {
            // Supprimer stock dans la bdd
            var dao = SelectStockById(stockId);
            if (dao.IdStock != null)
            {
                var query = "DELETE FROM public.stock WHERE \"idStock\"=:idStock";
                var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
                cmd.Parameters.AddWithValue("idStock", stockId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}