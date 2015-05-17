package com.lazylist;

import java.util.ArrayList;
import java.util.HashMap;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.dealsheel.R;
import com.utility.AppUtility;

public class Catlistviewadapter extends BaseAdapter {

	private Activity activity;
	ArrayList<HashMap<String, String>> data;
	private static LayoutInflater inflater = null;
	public ImageLoader imageLoader;
	AppUtility apputil;

	public Catlistviewadapter(Activity a, ArrayList<HashMap<String, String>> d) {
		activity = a;
		data = d;
		inflater = (LayoutInflater) activity
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		imageLoader = new ImageLoader(activity.getApplicationContext());
		apputil = new AppUtility(activity.getApplicationContext());
	}

	public int getCount() {
		return data.size();
	}

	public Object getItem(int position) {
		return position;
	}

	public long getItemId(int position) {
		return position;
	}

	public View getView(int position, View convertView, ViewGroup parent) {
		ViewHolder viewHoder;
		if (convertView == null) {
			convertView = inflater.inflate(R.layout.catlistview_listitem, null);
			viewHoder = new ViewHolder();
			viewHoder.itemImage = (ImageView) convertView
					.findViewById(R.id.img_catlistviewitem_image);
			viewHoder.name = (TextView) convertView
					.findViewById(R.id.tv_catlistviewitem_name);
			viewHoder.model = (TextView) convertView
					.findViewById(R.id.tv_catlistviewitem_model);
			viewHoder.price = (TextView) convertView
					.findViewById(R.id.tv_catlistviewitem_price);
			convertView.setTag(viewHoder);

		} else {
			viewHoder = (ViewHolder) convertView.getTag();
		}

		try {
			String tempurl = data.get(position).get("id");
			if (tempurl != null && !tempurl.equals("")) {
				try {
					imageLoader.DisplayImage(apputil.itemimagesurl + tempurl
							+ ".jpg", viewHoder.itemImage);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
			viewHoder.name.setText(data.get(position).get("Name"));
			viewHoder.model.setText(data.get(position).get("model"));
			viewHoder.price
					.setText("\u20B9 " + data.get(position).get("price"));
		} catch (Exception e) {
			e.printStackTrace();
		}
		return convertView;
	}

	static class ViewHolder {
		ImageView itemImage;
		TextView name;
		TextView model;
		TextView price;
	}
}
