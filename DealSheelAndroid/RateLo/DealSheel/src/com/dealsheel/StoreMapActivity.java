package com.dealsheel;

import java.util.HashMap;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.StrictMode;
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

public class StoreMapActivity extends Activity implements OnMapReadyCallback {

	GPSTracker gpsTracker;
	GpsTrackernew gpstrackernew;
	AppUtility apputil;
	String storeid, stateid, storename, storeimage, storelat, storelong,
			storephone, storeaddress;
	public ImageLoader imageLoader;
	TextView tvtitle, tvaddress, tvphone;
	ImageView imgsetting;
	HashMap<String, HashMap<String, String>> storeInfoList;

	@SuppressLint("NewApi")
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.storemapview);

		if (android.os.Build.VERSION.SDK_INT > 9) {
			StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder()
					.permitAll().build();
			StrictMode.setThreadPolicy(policy);
		}
		gpstrackernew = new GpsTrackernew(this);
		apputil = new AppUtility(StoreMapActivity.this);
		imageLoader = new ImageLoader(StoreMapActivity.this);
		tvtitle = (TextView) findViewById(R.id.tv_map_storetitle);
		tvaddress = (TextView) findViewById(R.id.tv_map_storeaddress);
		tvphone = (TextView) findViewById(R.id.tv_map_storephonenumber);
		imgsetting = (ImageView) findViewById(R.id.btn_map_left);
		storeInfoList = new HashMap<String, HashMap<String, String>>();
		Intent i = getIntent();
		storeid = i.getStringExtra("storeid");
		storename = i.getStringExtra("storename");
		storeimage = i.getStringExtra("storeimage");
		storelat = i.getStringExtra("storelat");
		storelong = i.getStringExtra("storelong");
		storephone = i.getStringExtra("storephone");
		storeaddress = i.getStringExtra("storeaddress");
		stateid = apputil.getstateid();
		tvtitle.setText(storename);
		tvaddress.setText(storeaddress);
		tvphone.setText(storephone);

		initilizeMap();

		imgsetting.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				startActivity(new Intent(getApplicationContext(),
						SettingActivity.class));
			}
		});
	}

	@SuppressLint({ "NewApi", "InflateParams" })
	private void initilizeMap() {
		((MapFragment) getFragmentManager().findFragmentById(R.id.map))
				.getMapAsync(this);

	}

	@Override
	public void onMapReady(GoogleMap googleMap) {
		// TODO Auto-generated method stub

		double latitude1 = gpstrackernew.latitude;
		double longitude1 = gpstrackernew.longitude;
		HashMap<String, String> storeInfo = new HashMap<String, String>();
		storeInfo.put("id", storeid);
		storeInfo.put("address", storeaddress);
		storeInfo.put("storeid", stateid);
		storeInfo.put("lat", storelat);
		storeInfo.put("long", storelong);
		storeInfo.put("phone", storephone);
		storeInfo.put("storename", storename);
		storeInfo.put("storeimage", storeimage);

		MarkerOptions marker = new MarkerOptions().position(
				new LatLng(latitude1, longitude1)).title("You Are Here");

		Marker userLocation = googleMap.addMarker(marker);
		Marker storeLocation = googleMap.addMarker(new MarkerOptions()
				.position(new LatLng(Double.valueOf(storelat), Double
						.valueOf(storelong))));
		storeInfoList.put(storeLocation.getId(), storeInfo);
		imageLoader.DisplayImage(storeimage, new ImageView(this));
		googleMap.setInfoWindowAdapter(new MapInfoWindowAdapter(this,
				storeInfoList,imageLoader));
		CameraPosition cameraPosition = new CameraPosition.Builder()
				.target(new LatLng(Double.valueOf(storelat), Double
						.valueOf(storelong))).zoom(10).build();

		googleMap.animateCamera(CameraUpdateFactory
				.newCameraPosition(cameraPosition));	
		userLocation.showInfoWindow();

	}
	//
	// public static Bitmap createDrawableFromView(Context context, View view) {
	// DisplayMetrics displayMetrics = new DisplayMetrics();
	// ((Activity) context).getWindowManager().getDefaultDisplay()
	// .getMetrics(displayMetrics);
	// view.setLayoutParams(new LayoutParams(LayoutParams.WRAP_CONTENT,
	// LayoutParams.WRAP_CONTENT));
	// view.measure(displayMetrics.widthPixels, displayMetrics.heightPixels);
	// view.layout(0, 0, displayMetrics.widthPixels,
	// displayMetrics.heightPixels);
	// view.buildDrawingCache();
	// Bitmap bitmap = Bitmap.createBitmap(view.getMeasuredWidth(),
	// view.getMeasuredHeight(), Bitmap.Config.ARGB_8888);
	//
	// Canvas canvas = new Canvas(bitmap);
	// view.draw(canvas);
	//
	// return bitmap;
	// }

}
