﻿using System.Collections.Generic;

namespace bidCardCoin.DAO
{
    public class ProduitDAO
    {
        // public string PhotoId { get; }

        public ProduitDAO()
        {
        }

        public ProduitDAO(string idProduit, string lotId, string utilisateurId, string stockId,
            string enchereGagnanteId, List<string> categorieId, string nomArtiste, string nomStyle, string nomProduit,
            double prixReserve, string referenceCatalogue, string descriptionProduit, bool isSend)
        {
            IdProduit = idProduit;
            LotId = lotId;
            UtilisateurId = utilisateurId;
            StockId = stockId;
            EnchereGagnanteId = enchereGagnanteId;
            CategorieId = categorieId;
            NomArtiste = nomArtiste;
            NomStyle = nomStyle;
            NomProduit = nomProduit;
            PrixReserve = prixReserve;
            ReferenceCatalogue = referenceCatalogue;
            DescriptionProduit = descriptionProduit;
            IsSend = isSend;
            // this.PhotoId = photoId;
        }

        public string IdProduit { get; }
        public string LotId { get; }
        public string UtilisateurId { get; }
        public string StockId { get; }
        public string EnchereGagnanteId { get; }
        public List<string> CategorieId { get; set; }
        public string NomArtiste { get; }
        public string NomStyle { get; }
        public string NomProduit { get; }
        public double PrixReserve { get; }
        public string ReferenceCatalogue { get; }
        public string DescriptionProduit { get; }
        public bool IsSend { get; }
    }
}