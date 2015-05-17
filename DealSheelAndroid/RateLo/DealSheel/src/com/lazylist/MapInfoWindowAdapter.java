package com.lazylist;

import java.util.HashMap;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.widget.ImageView;
import android.widget.TextView;

import com.dealsheel.R;
import com.google.android.gms.maps.GoogleMap.InfoWindowAdapter;
import com.google.android.gms.maps.model.Marker;

public class MapInfoWindowAdapter implements InfoWindowAdapter {
	private LayoutInflater layoutInflater;
	private HashMap<String, HashMap<String, String>> markerInfo;
	private ImageLoader imageLoader;
	private static View infoContents;

	public MapInfoWindowAdapter(Context context,
			HashMap<String, HashMap<String, String>> markerInfo,ImageLoader imageLoader) {
		// TODO Auto-generated constructor stub
		this.markerInfo = markerInfo;
		this.imageLoader=imageLoader;
		layoutInflater = LayoutInflater.from(context);		
	}

	@Override
	public View getInfoContents(Marker marker) {
		// TODO Auto-generated method stub
		infoContents = layoutInflater.inflate(R.layout.mapmarker, null);
		infoContents.setLayoutParams(new ViewGroup.LayoutParams(
				ViewGroup.LayoutParams.WRAP_CONTENT,
				ViewGroup.LayoutParams.WRAP_CONTENT));

		HashMap<String, String> thisMarkerInfo = markerInfo.get(marker.getId());
		ImageView storeimg = (ImageView) infoContents
				.findViewById(R.id.img_mareger_image);
		TextView tvtitle = (TextView) infoContents
				.findViewById(R.id.tv_marker_title);
		TextView tvaddress = (TextView) infoContents
				.findViewById(R.id.tv_marker_address);
		TextView tvprice = (TextView) infoContents
				.findViewById(R.id.tv_marker_price);
		TextView tvphone = (TextView) infoContents
				.findViewById(R.id.tv_marker_phone);
		if (thisMarkerInfo != null) {
			tvtitle.setText(thisMarkerInfo.get("storename"));
			tvaddress.setText(thisMarkerInfo.get("address"));
			tvphone.setText(thisMarkerInfo.get("phone"));
			try {
				imageLoader.DisplayImage(thisMarkerInfo.get("storeimage"),
						storeimg);
			} catch (Exception exception) {
				exception.printStackTrace();
			}
		} else {
			marker.setTitle("Your Location");
			return null;
		}
		return infoContents;
	}

	@Override
	public View getInfoWindow(Marker marker) {

		return null;
	}

}
