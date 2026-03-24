using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fahrzeugverwaltung.Models;

namespace Fahrzeugverwaltung
{
    public partial class MainWindow : Window
    {
        
        public ObservableCollection<Fahrzeug> Fahrzeuge { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            GridAutos.ItemsSource = Fahrzeuge;
            UpdateStatus();
        }

        private bool TryParseInputs(out Fahrzeug f)
        {
            f = null;

            if (string.IsNullOrWhiteSpace(TbKennzeichen.Text))
            { MessageBox.Show("Kennzeichen fehlt."); return false; }

            if (!int.TryParse(TbKm.Text, out var km) || km < 0)
            { MessageBox.Show("Kilometerstand ungültig."); return false; }

            if (!decimal.TryParse(TbPreis.Text, out var preis) || preis < 0)
            { MessageBox.Show("Preis ungültig."); return false; }

            var tuv = DpTuv.SelectedDate ?? DateTime.Today;

            f = new Fahrzeug
            {
                Kennzeichen = TbKennzeichen.Text.Trim(),
                Marke = TbMarke.Text.Trim(),
                Modell = TbModell.Text.Trim(),
                Kilometerstand = km,
                TuvDatum = tuv,
                Preis = preis
            };
            return true;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParseInputs(out var f)) return;

            if (Fahrzeuge.Any(x => string.Equals(x.Kennzeichen, f.Kennzeichen, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Kennzeichen existiert bereits.");
                return;
            }

            Fahrzeuge.Add(f);
            ClearInputs();
            UpdateStatus();
        }

        private void BtnLoadSelection_Click(object sender, RoutedEventArgs e)
        {
            if (GridAutos.SelectedItem is not Fahrzeug f) { MessageBox.Show("Kein Fahrzeug ausgewählt."); return; }

            TbKennzeichen.Text = f.Kennzeichen;
            TbMarke.Text = f.Marke;
            TbModell.Text = f.Modell;
            TbKm.Text = f.Kilometerstand.ToString();
            DpTuv.SelectedDate = f.TuvDatum;
            TbPreis.Text = f.Preis.ToString("0.##");
        }

        private void BtnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GridAutos.SelectedItem is not Fahrzeug selected) { MessageBox.Show("Kein Fahrzeug ausgewählt."); return; }
            if (!TryParseInputs(out var updated)) return;

            if (!string.Equals(selected.Kennzeichen, updated.Kennzeichen, StringComparison.OrdinalIgnoreCase) &&
                Fahrzeuge.Any(x => string.Equals(x.Kennzeichen, updated.Kennzeichen, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Neues Kennzeichen existiert bereits.");
                return;
            }

            selected.Kennzeichen = updated.Kennzeichen;
            selected.Marke = updated.Marke;
            selected.Modell = updated.Modell;
            selected.Kilometerstand = updated.Kilometerstand;
            selected.TuvDatum = updated.TuvDatum;
            selected.Preis = updated.Preis;

            GridAutos.Items.Refresh();
            UpdateStatus();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GridAutos.SelectedItem is not Fahrzeug f) { MessageBox.Show("Kein Fahrzeug ausgewählt."); return; }
            if (MessageBox.Show($"„{f.Kennzeichen}“ löschen?", "Löschen bestätigen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Fahrzeuge.Remove(f);
                UpdateStatus();
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) => ClearInputs();

        private void ClearInputs()
        {
            TbKennzeichen.Clear();
            TbMarke.Clear();
            TbModell.Clear();
            TbKm.Clear();
            TbPreis.Clear();
            DpTuv.SelectedDate = null;
        }

        private void UpdateStatus()
        {
            var sum = Fahrzeuge.Sum(x => x.Preis);
            TxtStatus.Text = $"Fahrzeuge: {Fahrzeuge.Count} | Gesamtwert: {sum:N2} €";
        }

    }
}
