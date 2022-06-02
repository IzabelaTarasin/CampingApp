using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp;
using System.Collections;
using System.Text;
using CampingApp_Server.Database;

namespace CampingApp_Server.Services
{
	public interface IPdfCreatorService
    {
        string CreatePdf(Reservation reservation);
    }

    public class PdfCreatorService : IPdfCreatorService
	{
		public PdfCreatorService()
		{
		}

        public string CreatePdf(Reservation reservation)
        {
            var path = Directory.GetCurrentDirectory();
            var pdfPath = $"{path}/confirm.pdf";
            var imagePath = $"{path}/Images/logo.png";
            var days = (reservation.EndDate - reservation.StartDate).Days;
            var totalPrice = days * reservation.Place.PricePerDay * reservation.NumberOfPeople;

            //create fonts
            var anandaRegular = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Andada-Regular.otf");
            var baseFontAnandaRegular = BaseFont.CreateFont(anandaRegular, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var fontRegular = new Font(baseFontAnandaRegular, 12, Font.NORMAL);
            var fontSmall = new Font(baseFontAnandaRegular, 8, Font.NORMAL);

            var anandaBold = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Andada-Bold.otf");
            var baseFontAnandaBold = BaseFont.CreateFont(anandaBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var fontBold = new Font(baseFontAnandaBold, 12, Font.NORMAL);
            var fontMainBold = new Font(baseFontAnandaBold, 15, Font.NORMAL);

            var stream = new FileStream(pdfPath, FileMode.Create);

            // step 1
            var document = new Document();
            document.SetPageSize(PageSize.A4);
            // step 2
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.PdfVersion = PdfWriter.VERSION_1_5;
            // step 3
            document.AddAuthor("CampApp");
            document.Open();

            PdfContentByte cb = writer.DirectContent;

            //create headers
            cb.BeginText();
            Phrase mainHeader = new Phrase($"Dziękujemy za dokonanie rezerwacji w obiekcie", fontMainBold);
            Phrase nameObjectHeader = new Phrase($"{ reservation.Place.Name}", fontMainBold);
            Phrase reservationNumberHeader = new Phrase($"Twój numer rezerwacji: {reservation.Id}", fontRegular);
            Phrase detailHeader = new Phrase("Szczegóły Twojej rezerwacji:", fontRegular);
            ColumnText.ShowTextAligned(
              cb, Element.ALIGN_LEFT,
              mainHeader,
              132, 646, 0
            );
            ColumnText.ShowTextAligned(
              cb, Element.ALIGN_LEFT,
              nameObjectHeader,
              260, 616, 0
            );
            ColumnText.ShowTextAligned(
              cb, Element.ALIGN_LEFT,
              reservationNumberHeader,
              82, 570, 0
            );
            ColumnText.ShowTextAligned(
              cb, Element.ALIGN_LEFT,
              detailHeader,
              82, 537, 0
            );
            cb.EndText();

            //craete table
            PdfPTable table = new PdfPTable(2);
            table.SpacingBefore = 200;
            table.TotalWidth = 432f;
            table.LockedWidth = true;

            PdfPCell cell;
            Phrase nameTitle = new Phrase("Nazwa obiektu: ", fontBold);
            cell = new PdfPCell(nameTitle) { MinimumHeight = 36f };
            table.AddCell(cell);
            Phrase name = new Phrase($"{reservation.Place.Name}", fontRegular);
            cell = new PdfPCell(name) { MinimumHeight = 36f };
            table.AddCell(cell);
            Phrase localisationTitle = new Phrase("Lokalizacja: ", fontBold);
            cell = new PdfPCell(localisationTitle) { MinimumHeight = 36f };
            table.AddCell(cell);
            Phrase localisation = new Phrase($"{reservation.Place.Address.ToString()}", fontRegular);
            cell = new PdfPCell(localisation);
            cell.FixedHeight = 100f;
            table.AddCell(cell);
            Phrase facilitiesTitle = new Phrase("Udogodnienia w obiekcie: ", fontBold);
            cell = new PdfPCell(facilitiesTitle) { MinimumHeight = 36f };
            table.AddCell(cell);
            Phrase facilities = new Phrase($"{GetFacilities(reservation.Place)}", fontRegular);
            cell = new PdfPCell(facilities);
            cell.FixedHeight = 100f;
            table.AddCell(cell);
            Phrase startDateTitle = new Phrase("Zameldowanie: ", fontBold);
            cell = new PdfPCell(startDateTitle) { MinimumHeight = 36f };
            table.AddCell(cell);
            Phrase startDate = new Phrase($"{reservation.StartDate}", fontRegular);
            cell = new PdfPCell(startDate) { MinimumHeight = 36f };
            table.AddCell(cell);
            Phrase endDateTitle = new Phrase("Wymeldowanie: ", fontBold);
            cell = new PdfPCell(endDateTitle) { MinimumHeight = 36f };
            table.AddCell(cell);
            Phrase endDate = new Phrase($"{reservation.EndDate}", fontRegular);
            cell = new PdfPCell(endDate) { MinimumHeight = 36f };
            table.AddCell(cell);
            Phrase numberOfPeopleTitle = new Phrase("Zarezerwowane dla: ", fontBold);
            cell = new PdfPCell(numberOfPeopleTitle) { MinimumHeight = 36f };
            table.AddCell(cell);
            if(reservation.NumberOfPeople == 1)
            {
                Phrase one = new Phrase($"{reservation.NumberOfPeople} osoby", fontRegular);
                cell = new PdfPCell(one) { MinimumHeight = 36f };
                table.AddCell(cell);
            } else
            {
                Phrase more = new Phrase($"{reservation.NumberOfPeople} osób", fontRegular);
                cell = new PdfPCell(more) { MinimumHeight = 36f };
                table.AddCell(cell);
            }
            if(reservation.Status.Id == 1){
                Phrase sumForActiveStatusTitle = new Phrase("Kwota do zapłaty: ", fontBold);
                cell = new PdfPCell(sumForActiveStatusTitle) { MinimumHeight = 36f };
                table.AddCell(cell);
                Phrase sumForActiveStatus = new Phrase($"{totalPrice} zł", fontRegular);
                cell = new PdfPCell(sumForActiveStatus) { MinimumHeight = 36f };
                table.AddCell(cell);
            }
            else if(reservation.Status.Id == 3)
            {
                Phrase sumForDoneStatusTitle = new Phrase("Status rezerwacji: ", fontBold);
                cell = new PdfPCell(sumForDoneStatusTitle) { MinimumHeight = 36f };
                table.AddCell(cell);
                Phrase sumForDoneStatus = new Phrase($"{reservation.Status.StatusName} - opłacono", fontRegular);
                cell = new PdfPCell(sumForDoneStatus) { MinimumHeight = 36f };
                table.AddCell(cell);
            }
            else
            {
                Phrase sumForCancelStatusTitle = new Phrase("Status rezerwacji: ", fontBold);
                cell = new PdfPCell(sumForCancelStatusTitle) { MinimumHeight = 36f };
                table.AddCell(cell);
                Phrase sumForCancelStatus = new Phrase($"{reservation.Status.StatusName}", fontRegular);
                cell = new PdfPCell(sumForCancelStatus) { MinimumHeight = 36f };
                table.AddCell(cell);
            }
            
            
            cb.SaveState();

            //create backkground
            PdfContentByte under = writer.DirectContentUnder;
            under.SaveState();
            under.SetRgbColorFill(0xAD, 0xC7, 0xBE); //crocodile tears
            //under.SetRgbColorFill(0xDB, 0xE3, 0xDE); //popielaty
            //under.SetRgbColorFill(0xFF, 0xEF, 0xD5);
            //under.SetRgbColorFill(0xC6, 0xCE, 0xCE); //golebi
            //under.SetRgbColorFill(0xDE, 0xD5, 0xD0); //siwy
            //under.SetRgbColorFill(0xE3, 0xE7, 0xE6); //mysi
            //under.SetRgbColorFill(0xF0, 0xF0, 0xF0); //ebonic
            //under.SetRgbColorFill(0xF2, 0xF2, 0xF2); //ZLAMANA BIEL
            //under.SetRgbColorFill(0xF5, 0xF1, 0xED); //ISABELLINE
            under.Rectangle(5, 5,
              PageSize.A4.Width - 10,
              PageSize.A4.Height - 10
            );
            under.Fill();
            under.RestoreState();

            //create logo
            var img = Image.GetInstance(imagePath);
            float w = img.ScaledWidth;
            float h = img.ScaledHeight;
            PdfTemplate t = writer.DirectContent.CreateTemplate(w, h);
            t.AddImage(img, w, 0, 0, h, 0, 0);
            Image clipped = Image.GetInstance(t);
            clipped.ScalePercent(100);
            document.Add(clipped);
            cb.RestoreState();

            document.Add(table);

            //create footer
            cb.BeginText();
            Phrase cancelText = new Phrase("Rezerwację możesz anulować po zalogowaniu na swoje konto", fontSmall);
            Phrase thankText = new Phrase("Dziękujemy za wybór aplikacji CampApp", fontSmall);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, cancelText, 200, 40, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, thankText, 230, 20, 0);
            cb.EndText();

            document.Close();
            stream.Dispose();

            return pdfPath;
        }

        private string GetFacilities(Place place)
        {
            var facilities = "";

            if (place.AnimalsAllowed)
            {
                facilities += "Zwierzęta mile widziane\n";
            }

            if (place.GrillExist)
            {
                facilities += "Grill\n";
            }

            if (place.MedicExist)
            {
                facilities += "Uslugi medyczne\n";
            }

            if (place.ReceptionExist)
            {
                facilities += "Recepcja całodobowa\n";
            }

            if (place.RestaurantExist)
            {
                facilities += "Restauracja\n";
            }

            if (place.SwimmingpoolExist)
            {
                facilities += "Basen\n";
            }

            if (place.WifiExist)
            {
                facilities += "WiFi\n";
            }

            return facilities;
        }
	}
}

