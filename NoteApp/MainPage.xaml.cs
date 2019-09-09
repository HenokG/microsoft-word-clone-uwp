using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Windows.UI;

namespace NoteApp
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            TxtArea.FontSize = 18;
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private async void SaveButtonPressed(object sender, PointerRoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we 
                // finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                TxtArea.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        private void UndoButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Document.Undo();
        }

        private void SuperScriptClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Superscript = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }
        private void SubScriptClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Subscript = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private async void OpenButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileOpenPicker open = new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".rtf");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                try
                {
                    Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    // Load the file into the Document property of the RichEditBox.
                    TxtArea.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                }
                catch (Exception ex)
                {
                    var dlg = new MessageDialog(ex.Message);
                    dlg.ShowAsync();
                }
            }
        }

        private void RedoButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Document.Redo();
        }

        private void SelectAllButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selection = TxtArea.Document.Selection;
            selection.StartPosition = 0;
            int end = TxtArea.Document.ToString().Length;
            selection.EndPosition = end;
        }

        private void CopyButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                selectedText.Copy();
            }
        }

        private void CutButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                selectedText.Cut();
            }
        }

        private void PasteButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                selectedText.Paste(1);
            }
        }

        private void DeleteButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                selectedText.Cut();
            }
        }

        private void FontButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            if (FontCombo.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                PopUpManager();
                FontCombo.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }
        
        private void UnderlineButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void StyleButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void BoldButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = TxtArea.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void FontSizeButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            if (slidingPopup.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                PopUpManager();
                slidingPopup.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                slidingPopup.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public void PopUpManager()
        {
            ItemsControl[] popups = { slidingPopup, ColorPicker1, ColorPicker2, ColorPicker3, FontCombo };
            foreach (ItemsControl popup in popups)
            {
                popup.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void FontColorClicked(object sender, PointerRoutedEventArgs e)
        {
            if (ColorPicker1.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                PopUpManager();
                ColorPicker1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ColorPicker2.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ColorPicker3.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                ColorPicker1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ColorPicker2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ColorPicker3.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void RichTextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void sizeSlideClicked(object sender, RangeBaseValueChangedEventArgs e)
        {
            TxtArea.FontSize = sizeSlide.Value;
        }

        #region Color Events
        private void LightGreenColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.LightGreen);
        }

        private void WhiteColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.White);
        }
        
        private void BlackColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void RedColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void DarkBlueColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.DarkBlue);
        }

        private void YellowColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.Yellow);
        }

        private void DarkGreenColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.DarkGreen);
        }

        private void BrownColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.Brown);
        }

        private void PinkColorClicked(object sender, PointerRoutedEventArgs e)
        {
            TxtArea.Foreground = new SolidColorBrush(Colors.Pink);
        }
        #endregion

        private void PrintClicked(object sender, PointerRoutedEventArgs e)
        {
            var lg = new MessageDialog("No Printer Attached To The Machine");
            lg.ShowAsync();
        }

        private void papyrusClicked(object sender, PointerRoutedEventArgs e)
        {
            
        }

    }
}
