﻿using System.Collections.Generic;
using bidCardCoin.DAL;
using BidCardCoin.Models;

namespace bidCardCoin.ORM
{
    public class PaiementORM
    {
        private static readonly Dictionary<string, Paiement> _paiementsDictionary = new Dictionary<string, Paiement>();

        private static bool PaiementAlreadyInDictionary(string id)
        {
            return _paiementsDictionary.ContainsKey(id);
        }
        // todo -> liens vers un : Utilisateur // Lot

        public static void Populate(List<Paiement> paiements)
        {
            // liste des paiements qui on beusoin de se faire peupler (leurs liste utilisateurs)

            foreach (var paiement in paiements)
            {
                if (!PaiementAlreadyInDictionary(paiement.IdPaiement)) GetPaiementById(paiement.IdPaiement);

                paiement.UtilisateurPaiement = _paiementsDictionary[paiement.IdPaiement].UtilisateurPaiement;
                paiement.LotPaiement = _paiementsDictionary[paiement.IdPaiement].LotPaiement;
            }
        }

        public static void Populate(Paiement paiement)
        {
            // liste des paiements qui on beusoin de se faire peupler (leurs liste utilisateurs)


            if (!PaiementAlreadyInDictionary(paiement.IdPaiement)) GetPaiementById(paiement.IdPaiement);

            paiement.UtilisateurPaiement = _paiementsDictionary[paiement.IdPaiement].UtilisateurPaiement;
            paiement.LotPaiement = _paiementsDictionary[paiement.IdPaiement].LotPaiement;
        }

        public static Paiement GetPaiementById(string id, bool initializer = true)
        {
            var pdao = PaiementDAL.SelectPaiementById(id);
            var utilisateurPaiement = new Utilisateur();
            var lotPaiement = new Lot();


            if (initializer)
            {
                lotPaiement = LotORM.GetLotById(LotDAL.SelectLotById(pdao.LotId).IdLot, false);
                utilisateurPaiement =
                    UtilisateurORM.GetUtilisateurById(
                        UtilisateurDAL.SelectUtilisateurById(pdao.UtilisateurId).IdUtilisateur, false);
            }


            var paiement = new Paiement(pdao.IdPaiement, utilisateurPaiement, pdao.TypePaiement,
                pdao.ValidationPaiement, lotPaiement);

            if (initializer)
            {
                _paiementsDictionary[paiement.IdPaiement] = paiement;
                LotORM.Populate(paiement.LotPaiement);
                UtilisateurORM.Populate(new List<Utilisateur>(new[]
                {
                    paiement.UtilisateurPaiement
                }));
            }

            return paiement;
        }

        public static List<Paiement> GetAllPaiement()
        {
            var lpdao = PaiementDAL.SelectAllPaiement();
            var paiements = new List<Paiement>();

            foreach (var pdao in lpdao) paiements.Add(GetPaiementById(pdao.IdPaiement));

            return paiements;
        }
    }
}