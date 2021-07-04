using System;
using Xamarin.Essentials;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


namespace CosmicWatch_Library
{
    public class GeolocationString
    {
        //To cancel this task.
        private CancellationTokenSource cancelAttempt;
        
        public GeolocationString()
        {
            //mapControl.MapServiceToken = "xLElw17kpzYSV0LEFpnh ~nhVDrZ2pvQH03Z4uyX3q5w~AnIj13DgaYie8hIcbWkmyyo_ox4X4yne28Uq1bCW0vMAQQNEPvHMFoo088DBJThK";
        }

        public async Task<String> GetCurrentLocation() {
            try {
                GeolocationRequest locationRequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                cancelAttempt = new CancellationTokenSource();
                Location location = await Geolocation.GetLocationAsync(locationRequest, cancelAttempt.Token);

                if (location != null) {
                    if (location.IsFromMockProvider) {
                        return "Mock Location!";
                    }
                    else {
                        return ($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    }

                }
                return "Not sure how it got to here.";
            }
            catch (FeatureNotSupportedException) {
                //Not supported on device
                return "Feature not supported";
            }
            catch (FeatureNotEnabledException) {
                //Not enabled on device
                return "Feature not enabled";
            }
            catch (PermissionException) {
                //Permission needed
                return "Need Permission";
            }
            catch (Exception) {
                // Unable to get location
                return "Cannot get location";
            }
        }

        public void OnDisappearing() {
            if (cancelAttempt != null && !cancelAttempt.IsCancellationRequested)
                cancelAttempt.Cancel();
        }

        public async Task<String> GetPositionFromLocation(Location location) {
            try {

                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null) {
                    var geocodeAddress =
                        $"AdminArea:       {placemark.AdminArea}\n" +
                        $"CountryCode:     {placemark.CountryCode}\n" +
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"FeatureName:     {placemark.FeatureName}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"PostalCode:      {placemark.PostalCode}\n" +
                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                        $"SubLocality:     {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare:    {placemark.Thoroughfare}\n";

                    return geocodeAddress;
                }
                else {
                    return "Placemark is null.";
                }
            }
            catch (FeatureNotSupportedException) {
                return "Placemark feature not supported.";
            }
            catch (Exception) {
                return "Placemark cannot be found, exception thrown, Bing Maps token probably not set.";
            }
        }

        private CancellationTokenSource cancelAttemptPosition;
        public delegate void ChangeStringDisplay(String display);
        public async Task<String> GetCurrentLocationName(ChangeStringDisplay stringFunc)
        {
            try
            {
                GeolocationRequest locationRequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                cancelAttemptPosition = new CancellationTokenSource();
                Location location = await Geolocation.GetLocationAsync(locationRequest, cancelAttemptPosition.Token);

                if (location != null)
                {
                    if (location.IsFromMockProvider)
                    {
                        return "Mock Location!";
                    }
                    else
                    {
                        stringFunc(await GetPositionFromLocation(location));
                        return await GetPositionFromLocation(location);
                    }

                }
                return "Not sure how it got to here.";
            }
            catch (FeatureNotSupportedException)
            {
                //Not supported on device
                return "Feature not supported";
            }
            catch (FeatureNotEnabledException)
            {
                //Not enabled on device
                return "Feature not enabled";
            }
            catch (PermissionException)
            {
                //Permission needed
                return "Need Permission";
            }
            catch (Exception)
            {
                // Unable to get location
                return "Cannot get location";
            }
        }

        public void OnDisappearingPosition()
        {
            if (cancelAttemptPosition != null && !cancelAttemptPosition.IsCancellationRequested)
                cancelAttemptPosition.Cancel();
        }

    }
}
