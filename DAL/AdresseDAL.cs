﻿using System;
using System.Collections.Generic;
using bidCardCoin.DAO;
using Npgsql;

namespace bidCardCoin.DAL
{
    public static class AdresseDAL
    {
        // SELECT LISTE PERSONNE ON ADRESSE BY ID
        public static List<string> SelectPersonneInAdressesById(string id)
        {
            // récup la liste des personnes
            var query =
                "SELECT * FROM public.adressepersonne ap where ap.\"adresseId\"=:idAdresseParam";
            var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
            cmd.Parameters.AddWithValue("idAdresseParam", id);

            var listeIdPersonne = new List<string>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var personneId = (string) reader["personneId"];
                listeIdPersonne.Add(personneId);
            }

            reader.Close();
            return listeIdPersonne;
        }

        // SELECT
        public static AdresseDAO SelectAdresseById(string id)
        {
            var adresseDao = new AdresseDAO();
            // Selectionne l'adresse a partir de l'id
            var query =
                "SELECT * FROM public.adresse a where a.\"idAdresse\"=:idAdresseParam order by a.\"idAdresse\" desc";
            var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
            cmd.Parameters.AddWithValue("idAdresseParam", id);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // récup les paramètres principaux
                var idAdresse = (string) reader["idAdresse"];

                var pays = Convert.IsDBNull(reader["pays"]) ? null : (string) reader["pays"];
                var region = Convert.IsDBNull(reader["region"]) ? null : (string) reader["region"];
                var ville = Convert.IsDBNull(reader["ville"]) ? null : (string) reader["ville"];
                var codePostal = Convert.IsDBNull(reader["codePostal"]) ? null : (string) reader["codePostal"];
                var adresse = Convert.IsDBNull(reader["adresseNom"]) ? null : (string) reader["adresseNom"];
                adresseDao = new AdresseDAO(idAdresse, pays, region, ville, codePostal, adresse,
                    new List<string>());
            }

            reader.Close();

            // si nous avons une id adresse, alors nous avons une adresse présente dans la bdd, dans le cas contraire
            // on retourne un DAO vide
            if (adresseDao.IdAdresse != null)
            {
                adresseDao.ListePersonneId = SelectPersonneInAdressesById(id);
                return adresseDao;
            }

            return new AdresseDAO();
        }

        public static List<AdresseDAO> SelectAllAdresse()
        {
            // Selectionné tout les adresse dans la base de donnée
            var liste = new List<AdresseDAO>();

            var query = "SELECT * FROM public.adresse ORDER BY \"idAdresse\"";
            var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var idAdresse = (string) reader["idAdresse"];

                var pays = Convert.IsDBNull((string) reader["pays"]) ? null : (string) reader["pays"];
                var region = Convert.IsDBNull((string) reader["region"]) ? null : (string) reader["region"];
                var ville = Convert.IsDBNull((string) reader["ville"]) ? null : (string) reader["ville"];
                var codePostal = Convert.IsDBNull((string) reader["codePostal"]) ? null : (string) reader["codePostal"];
                var adresse = Convert.IsDBNull((string) reader["adresseNom"]) ? null : (string) reader["adresseNom"];


                liste.Add(new AdresseDAO(idAdresse, pays, region, ville, codePostal, adresse,
                    new List<string>()));
            }

            reader.Close();
            foreach (var adresseDao in liste)
                adresseDao.ListePersonneId = SelectPersonneInAdressesById(adresseDao.IdAdresse);

            reader.Close();

            return liste;
        }

        // INSERT & Update 
        public static void InsertOrAddNewAdresse(AdresseDAO adresse)
        {
            // Inserer adresse dans la bdd
            var query =
                "INSERT INTO public.adresse (\"idAdresse\",\"pays\",\"region\",\"ville\",\"codePostal\", \"adresseNom\" ) values (:idAdresse,:pays,:region,:ville,:codePostal,:adresseNom) ON CONFLICT ON CONSTRAINT pk_adresse DO UPDATE SET  \"idAdresse\"=:idAdresse,  \"pays\"=:pays,  \"region\"=:region,  \"ville\"=:ville,  \"codePostal\"=:codePostal,  \"adresseNom\"=:adresseNom where adresse.\"idAdresse\"=:idAdresse ";
            var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
            cmd.Parameters.AddWithValue("idAdresse", adresse.IdAdresse);
            cmd.Parameters.AddWithValue("pays", adresse.Pays);
            cmd.Parameters.AddWithValue("region", adresse.Region);
            cmd.Parameters.AddWithValue("ville", adresse.Ville);
            cmd.Parameters.AddWithValue("codePostal", adresse.CodePostal);
            cmd.Parameters.AddWithValue("adresseNom", adresse.Adresse);
            cmd.ExecuteNonQuery();

            foreach (var elemPersonneId in adresse.ListePersonneId)
            {
                query =
                    "INSERT INTO public.adressepersonne values (:idAdresse,:idPersonne) ON CONFLICT  DO NOTHING";
                cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
                cmd.Parameters.AddWithValue("idAdresse", adresse.IdAdresse);
                cmd.Parameters.AddWithValue("idPersonne", elemPersonneId);
                cmd.ExecuteNonQuery();
            }
        }

// DELETE
        public static void DeleteAdresse(string adresseId)
        {
            // Supprimer adresse dans la bdd
            var dao = SelectAdresseById(adresseId);
            if (dao.IdAdresse != null)
            {
                var query = "DELETE FROM public.adresse WHERE \"idAdresse\"=:idAdresse";
                var cmd = new NpgsqlCommand(query, DALconnection.OpenConnection());
                cmd.Parameters.AddWithValue("idAdresse", adresseId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}