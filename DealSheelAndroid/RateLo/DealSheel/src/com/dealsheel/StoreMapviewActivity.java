package com.dealsheel;

import java.util.ArrayList;
import java.util.HashMap;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.StrictMode;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ImageView;
import android.widget.TextView;

import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapFragment;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.model.CameraPosition;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import com.lazylist.ImageLoader;
import com.lazylist.MapInfoWindowAdapter;
import com.utility.AppUtility;
import com.utility.GPSTracker;
import com.utility.GpsTrackernew;
import com.utility.ServiceHandler;

public class StoreMapviewActivity extends Activity implements
		OnMapReadyCallback {

	GPSTracker gpsTracker;
	GpsTrackernew gpstrackernew;
	ArrayList<HashMap<String, String>> storelist;
	AppUtility apputil;
	private ProgressDialog pDialog;
	String storeid, stateid, storename, storeimage;
	public ImageLoader imageLoader;
	TextView tvtitle, tvaddress, tvphone;
	ImageView imgsetting;
	HashMap<String, HashMap<String, String>> markerinfo;
	private Context actvityContext;

	@SuppressLint("NewApi")
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.storemapview);

		if (android.os.Build.VERSION.SDK_INT > 9) {
			StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder()
					.permitAll().build();
			StrictMode.setThreadPolicy(policy);
		}
		actvityContext = this;
		gpsTracker = new GPSTracker(this);
		gpstrackernew = new GpsTrackernew(this);
		apputil = new AppUtility(StoreMapviewActivity.this);
		storelist = new ArrayList<HashMap<String, String>>();
		tvtitle = (TextView) findViewById(R.id.tv_map_storetitle);
		tvaddress = (TextView) findViewById(R.id.tv_map_storeaddress);
		tvphone = (TextView) findViewById(R.id.tv_map_storephonenumber);
		imgsetting = (ImageView) findViewById(R.id.btn_map_left);

		Intent i = getIntent();
		storeid = i.getStringExtra("storeid");
		storename = i.getStringExtra("storename");
		storeimage = i.getStringExtra("storeimage");
		stateid = apputil.getstateid();

		tvtitle.setText(storename);
		new GetContacts().execute();

		imgsetting.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				startActivity(new Intent(getApplicationContext(),
						SettingActivity.class));
			}
		});

	}

	private class GetContacts extends AsyncTask<Void, Void, Void> {

		String sid, sname, sctime, slupdate, slat, longdesc;

		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			pDialog = new ProgressDialog(StoreMapviewActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance

			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.storedeatilurl
					+ storeid + "&Stateid=" + stateid, ServiceHandler.GET);

			Log.e("store Response: ", "> " + jsonStr);

			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);

					for (int i = 0; i < contacts.length(); i++) {
						JSONObject c = contacts.getJSONObject(i);
						sid = c.getString("id");
						String saddress = c.getString("Address");
						String sstoreid = c.getString("StoreId");
						String slatitute = c.getString("Lattitude");
						String slongitute = c.getString("Longitude");
						String sphone = c.getString("PhoneNumber");

						HashMap<String, String> map = new HashMap<String, String>();
						map.put("id", sid);
						map.put("address", saddress);
						map.put("storeid", sstoreid);
						map.put("lat", slatitute);
						map.put("long", slongitute);
						map.put("phone", sphone);
						storelist.add(map);
					}
				} catch (JSONException e) {
					e.printStackTrace();
				}
			} else {
				Log.e("ServiceHandler", "Couldn't get any data from the url");
			}

			return null;
		}

		@Override
		protected void onPostExecute(Void result) {
			super.onPostExecute(result);

			if (pDialog.isShowing())
				pDialog.dismiss();
			initilizeMap();
		}

	}

	@SuppressLint({ "NewApi", "InflateParams" })
	private void initilizeMap() {
		((MapFragment) getFragmentManager().findFragmentById(R.id.map))
				.getMapAsync(this);
	}

	@Override
	public void onMapReady(GoogleMap googleMap) {
		// TODO Auto-generated method stub

		double myLatitude = gpstrackernew.latitude;
		double myLongitude = gpstrackernew.longitude;
		MarkerOptions userLocationMarkerOptions = new MarkerOptions().position(
				new LatLng(myLatitude, myLongitude)).title("You Are Here");
		Marker userLocationMaerker = googleMap
				.addMarker(userLocationMarkerOptions);
		markerinfo = new HashMap<String, HashMap<String, String>>();
		CameraPosition cameraPosition = new CameraPosition.Builder()
				.target(new LatLng(myLatitude, myLongitude)).zoom(10).build();
		googleMap.animateCamera(CameraUpdateFactory
				.newCameraPosition(cameraPosition));
		imageLoader = new ImageLoader(this);
		imageLoader.DisplayImage(storeimage, new ImageView(this));
		for (int i = 0; i < storelist.size(); i++) {

			double latitude = Double.valueOf(storelist.get(i).get("lat"));
			double longitude = Double.valueOf(storelist.get(i).get("long"));
			Marker marker = googleMap.addMarker(new MarkerOptions().position(
					new LatLng(latitude, longitude)).title(storename));
			storelist.get(i).put("storename", storename);
			storelist.get(i).put("storeimage", storeimage);
			String id = marker.getId();
			markerinfo.put(id, storelist.get(i));
		}

		MapInfoWindowAdapter infosAdapter = new MapInfoWindowAdapter(
				actvityContext, markerinfo, imageLoader);
		googleMap.setInfoWindowAdapter(infosAdapter);
		userLocationMaerker.showInfoWindow();

	}

}
