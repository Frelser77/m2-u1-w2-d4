using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace m2_u1_w2_d4
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CaricaAutomobili();
                CaricaAnniGaranzia();
                CaricaOptional();
            }
        }

        // metodo per gestire il click del bottone e calcolare il preventivo
        /* protected void BtnCalcola_Click(object sender, EventArgs e)
         {
             int autoId;
             if (int.TryParse(Automobili.SelectedValue, out autoId))
             {
                 autoId = int.Parse(Automobili.SelectedValue);
                 decimal prezzoBase = TrovaPrezzoBaseAuto(autoId);
                 decimal prezzoOptional = CalcolaPrezzoOptional();
                 int anniGaranzia = int.Parse(ddlAnniGaranzia.SelectedValue);
                 decimal prezzoGaranzia = anniGaranzia * 120;
                 decimal prezzoTotale = prezzoBase + prezzoOptional + prezzoGaranzia;

                 // Aggiorna il path dell'immagine
                 string selectedImagePath = GetImagePathFromDatabase(autoId);
                 imgAuto.ImageUrl = !string.IsNullOrEmpty(selectedImagePath) ? ResolveUrl(selectedImagePath) : ResolveUrl("~/Content/images/default.png");

                 // Aggiorna i campi della card con i dettagli del preventivo
                 prezzoBaseLiteral.Text = prezzoBase == 0 ? string.Empty : $"Prezzo di base del modello scelto: {prezzoBase:C}";
                 prezzoOptionalLiteral.Text = prezzoOptional == 0 ? string.Empty : $"Totale degli optional scelti: {prezzoOptional:C}";
                 prezzoGaranziaLiteral.Text = $"Totale della garanzia ({anniGaranzia} anni): {prezzoGaranzia:C}";
                 prezzoTotaleLiteral.Text = $"<strong>Totale complessivo: {prezzoTotale:C}</strong>";


                 // Rendi visibile la card
                 preventivoCard.Visible = true;
             }
             else
             {
                 // Gestisci il caso in cui non viene selezionata un'auto valida
                 // Potresti, ad esempio, visualizzare un messaggio di errore
                 testoPreventivo.Text = "Per favore, seleziona un modello di auto valido.";
                 preventivoCard.Visible = false; // Nascondi la card del preventivo se non è valida la selezione
             }
         }*/



        // Metodo per recuperare il path dell'immagine dell'auto dal database
        private string GetImagePathFromDatabase(int autoId)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConcessionariaConnectionString"].ConnectionString;
            string imagePath = "";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ImmaginePath FROM Automobili WHERE ID = @AutoID", conn);
                cmd.Parameters.AddWithValue("@AutoID", autoId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    imagePath = result.ToString();
                }
            }
            return imagePath;
        }

        // Metodo per calcolare il prezzo degli optional selezionati
        private decimal CalcolaPrezzoOptional()
        {
            decimal prezzoOptional = 0;
            foreach (ListItem item in CheckBoxListOptional.Items)
            {
                if (item.Selected)
                {
                    int optionalId = int.Parse(item.Value);
                    prezzoOptional += TrovaPrezzoOptional(optionalId);
                }
            }
            return prezzoOptional;
        }

        // Metodo per caricare le automobili dal database e popolare la dropdownlist
        private void CaricaAutomobili()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConcessionariaConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, NomeModello, ImmaginePath FROM Automobili", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Automobili.Items.Clear();
                while (reader.Read())
                {
                    ListItem item = new ListItem(reader["NomeModello"].ToString(), reader["ID"].ToString());
                    item.Attributes["data-img-path"] = reader["ImmaginePath"].ToString();
                    Automobili.Items.Add(item);
                }
            }
        }

        // Metodo per caricare gli anni di garanzia nella dropdownlist 
        private void CaricaAnniGaranzia()
        {
            for (int i = 1; i <= 5; i++)
            {
                ddlAnniGaranzia.Items.Add(i.ToString());
            }
        }

        // Metodo per caricare gli optional dal database e popolare la checkboxlist
        private void CaricaOptional()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConcessionariaConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, Descrizione FROM Optional", conn);
                conn.Open();
                CheckBoxListOptional.DataSource = cmd.ExecuteReader();
                CheckBoxListOptional.DataTextField = "Descrizione";
                CheckBoxListOptional.DataValueField = "ID";

                CheckBoxListOptional.DataBind();

                foreach (ListItem item in CheckBoxListOptional.Items)
                {
                    item.Attributes.Add("onclick", "document.getElementById('" + btnHiddenForOptional.ClientID + "').click();");

                }
            }
        }

        private void AggiornaPreventivo()
        {
            int autoId = int.Parse(Automobili.SelectedValue);
            decimal prezzoBase = TrovaPrezzoBaseAuto(autoId);
            decimal prezzoOptional = CalcolaPrezzoOptional();
            int anniGaranzia;
            decimal prezzoGaranzia = 0;

            if (int.TryParse(ddlAnniGaranzia.SelectedValue, out anniGaranzia))
            {
                prezzoGaranzia = anniGaranzia * 120;
            }

            decimal prezzoTotale = prezzoBase + prezzoOptional + prezzoGaranzia;

            // Aggiorna il path dell'immagine
            string selectedImagePath = GetImagePathFromDatabase(autoId);
            imgAuto.ImageUrl = !string.IsNullOrEmpty(selectedImagePath) ? ResolveUrl(selectedImagePath) : ResolveUrl("~/Content/images/default.png");

            // Aggiorna i campi della card con i dettagli del preventivo
            prezzoBaseLiteral.Text = $"Prezzo di base del modello scelto: {prezzoBase:C}";
            prezzoOptionalLiteral.Text = $"Totale degli optional scelti: {prezzoOptional:C}";
            prezzoGaranziaLiteral.Text = $"Totale della garanzia ({anniGaranzia} anni): {prezzoGaranzia:C}";
            prezzoTotaleLiteral.Text = $"<strong>Totale complessivo: {prezzoTotale:C}</strong>";

            // Rendi visibile la card del preventivo
            preventivoCard.Visible = true;
        }


        protected void UpdatePreventivo(object sender, EventArgs e)
        {
            AggiornaPreventivo();
        }

        protected void CheckBoxListOptional_SelectedIndexChanged(object sender, EventArgs e)
        {
            AggiornaPreventivo();
        }

        protected void ddlAnniGaranzia_SelectedIndexChanged(object sender, EventArgs e)
        {
            AggiornaPreventivo();
        }


        // Metodo per trovare il prezzo base dell'auto selezionata
        private decimal TrovaPrezzoBaseAuto(int autoId)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConcessionariaConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT PrezzoBase FROM Automobili WHERE ID = @AutoID", conn);
                cmd.Parameters.AddWithValue("@AutoID", autoId);
                conn.Open();
                return (decimal)cmd.ExecuteScalar();
            }
        }

        // Metodo per trovare il prezzo dell'optional selezionato
        private decimal TrovaPrezzoOptional(int optionalId)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConcessionariaConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Prezzo FROM Optional WHERE ID = @OptionalID", conn);
                cmd.Parameters.AddWithValue("@OptionalID", optionalId);
                conn.Open();
                return (decimal)cmd.ExecuteScalar();
            }
        }

        // Metodo per gestire la selezione di un'auto dalla dropdownlist e pulire i campi
        protected void Automobili_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Resetta la selezione per gli optional e gli anni di garanzia
            CheckBoxListOptional.ClearSelection();
            ddlAnniGaranzia.ClearSelection();

            // Ottieni l'ID dell'auto selezionata
            int autoId = int.Parse(Automobili.SelectedValue);

            // Ottieni il prezzo base dell'auto selezionata
            decimal prezzoBase = TrovaPrezzoBaseAuto(autoId);

            // Imposta l'URL dell'immagine
            string selectedImagePath = GetImagePathFromDatabase(autoId);
            imgAuto.ImageUrl = !string.IsNullOrEmpty(selectedImagePath) ? ResolveUrl(selectedImagePath) : ResolveUrl("~/Content/images/default.png");

            // Imposta il prezzo base e resetta i campi degli optional e della garanzia ai valori di default
            prezzoBaseLiteral.Text = $"Prezzo di base del modello scelto: {prezzoBase:C}";
            prezzoOptionalLiteral.Text = "Totale degli optional scelti: €0,00";
            prezzoGaranziaLiteral.Text = "Totale della garanzia: €0,00";
            prezzoTotaleLiteral.Text = $"<strong>Totale complessivo: {prezzoBase:C}</strong>";

            // Rendi visibile la card del preventivo
            preventivoCard.Visible = true;
        }



    }
}
