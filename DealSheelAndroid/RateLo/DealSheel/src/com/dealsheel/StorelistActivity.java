package com.dealsheel;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v4.widget.SlidingPaneLayout.PanelSlideListener;
import android.util.Log;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnKeyListener;
import android.view.inputmethod.InputMethodManager;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ExpandableListView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import com.lazylist.ImageLoader;
import com.lazylist.ItemStoreadapter;
import com.lazylist.RightMenuAdapter;
import com.navdrawer.SimpleSideDrawer;
import com.utility.AppUtility;
import com.utility.GPSTracker;
import com.utility.ServiceHandler;

public class StorelistActivity extends Activity implements OnClickListener {

	ImageView imgsearch, imgmap, imggeneric, itemimage, imgsetting,
			imgsearchbtn, imgrightclose, imgleftclose;
	TextView itemname, itemmodel, itemdesc, tvheadertitle;
	EditText edsearch;
	ExpandableListView storeListView;
	ArrayList<HashMap<String, String>> storelist;
	AppUtility apputil;
	private ProgressDialog pDialog;
	String rid, rtitle, rdesc, rmodel, rimage, searchtext;
	GPSTracker gps;
	String latitude, longitude;
	ItemStoreadapter adapter;
	public ImageLoader imageLoader;
	LinearLayout lnvmain, lnvsearched;
	Button btnclear;;
	SimpleSideDrawer slide_me;
	// ExpandableListView lv_rightmenu;
	// RightMenuAdapter right_adapter;
	// ArrayList<HashMap<String, String>> rightmenulist;
	String getfilterIsint, getfilteralias;
	private Button sortByPrice;
	private Button sortByDistance;

	private enum SortBy {
		PRICE, DISTANCE
	};

	@Override
	protected void onCreate(Bundle savedInstanceState) {

		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.storelist);
		// rightmenulist = new ArrayList<HashMap<String, String>>();
		edsearch = (EditText) findViewById(R.id.ed_storelist_search);
		imgsearchbtn = (ImageView) findViewById(R.id.img_storelist_searchbtn);
		lnvmain = (LinearLayout) findViewById(R.id.lnv_storelist_itemview);
		imgsearch = (ImageView) findViewById(R.id.img_storelist_search);
		imggeneric = (ImageView) findViewById(R.id.img_storelist_generic);
		itemimage = (ImageView) findViewById(R.id.img_storelist_itemimage);
		imgsetting = (ImageView) findViewById(R.id.btn_storelist_left);
		itemname = (TextView) findViewById(R.id.tv_storelist_itemtitle);
		itemmodel = (TextView) findViewById(R.id.tv_storelist_itemmodel);
		tvheadertitle = (TextView) findViewById(R.id.tv_storelist_headertitle);
		itemdesc = (TextView) findViewById(R.id.tv_storelist_itemdesc);

		apputil = new AppUtility(StorelistActivity.this);
		storelist = new ArrayList<HashMap<String, String>>();
		imageLoader = new ImageLoader(getApplicationContext());

		storeListView = (ExpandableListView) findViewById(R.id.elv_itemstorelist);

		slide_me = new SimpleSideDrawer(this);
		slide_me.setLeftBehindContentView(R.layout.leftpanel);
		slide_me.setRightBehindContentView(R.layout.right_menu_sort);

		// lv_rightmenu = (ExpandableListView) findViewById(R.id.elv_rightmenu);
		imgleftclose = (ImageView) findViewById(R.id.img_leftmenu_close);
		imgrightclose = (ImageView) findViewById(R.id.iv_close_drawer_right);

		sortByDistance = (Button) slide_me.findViewById(R.id.btn_by_distance);
		sortByPrice = (Button) slide_me.findViewById(R.id.btn_by_price);

		sortByDistance.setOnClickListener(this);
		sortByPrice.setOnClickListener(this);

		lnvsearched = (LinearLayout) findViewById(R.id.lnv_searchbar_edittext);
		btnclear = (Button) findViewById(R.id.btn_search_cleartext);
		btnclear.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				edsearch.setText("");
			}
		});

		imgleftclose.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				slide_me.toggleLeftDrawer();
			}
		});

		imgrightclose.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				slide_me.toggleRightDrawer();
			}
		});
		gps = new GPSTracker(StorelistActivity.this);

		latitude = String.valueOf(gps.getLatitude());
		longitude = String.valueOf(gps.getLongitude());
		Log.e("lat&long", latitude + ":" + longitude);

		try {
			Intent i = getIntent();
			int tint = i.getIntExtra("flag", 0);
			Log.e("receive flag==>", tint + "");

			/*
			 * if(tint == 0) {
			 */
			rid = i.getStringExtra("id");
			rtitle = i.getStringExtra("title");
			rdesc = i.getStringExtra("desc");
			rmodel = i.getStringExtra("model");
			imageLoader.DisplayImage(apputil.itemimagesurl + rid + ".jpg",
					itemimage);
			Log.e("receive param:--", rid + ":" + rtitle + ":" + rdesc + ":"
					+ rmodel);
			itemname.setText(rtitle);
			itemmodel.setText(rmodel);
			itemdesc.setText(rdesc);

		} catch (Exception e) {
			e.printStackTrace();
		}

		new GetStoreJSONTask().execute();

		// lv_rightmenu.setOnItemClickListener(new OnItemClickListener() {
		//
		// @Override
		// public void onItemClick(AdapterView<?> parent, View view,
		// int position, long id) {
		// // TODO Auto-generated method stub
		//
		// getfilteralias = rightmenulist.get(position).get("alias");
		// getfilterIsint = rightmenulist.get(position).get("isint");
		// new Getfiltervalues().execute();
		// }
		// });

		TextView tvs = (TextView) findViewById(R.id.tv_leftpanel_setting);
		tvs.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				slide_me.toggleLeftDrawer();
				startActivity(new Intent(getApplicationContext(),
						SettingActivity.class));
			}
		});

		TextView tvrateus = (TextView) findViewById(R.id.tv_leftpanel_Rateus);
		tvrateus.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				Intent browserIntent = new Intent(
						Intent.ACTION_VIEW,
						Uri.parse("https://play.google.com/store/apps/details?id=com.dealsheel"));
				startActivity(browserIntent);
			}
		});

		TextView tvcontactus = (TextView) findViewById(R.id.tv_leftpanel_contact);
		tvcontactus.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				 Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://www.ratelo.com:8080/DealSheelAppServer/")); 
				 startActivity(browserIntent);
				 
			}
		});

		imgsetting.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				slide_me.toggleLeftDrawer();
				/**/
			}
		});

		imggeneric.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				slide_me.toggleRightDrawer();
			}
		});

		imgsearch.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				lnvsearched.setVisibility(View.VISIBLE);
				imgsearch.setVisibility(View.GONE);
				tvheadertitle.setVisibility(View.GONE);
				imgsearchbtn.setVisibility(View.VISIBLE);
				edsearch.requestFocus();
				InputMethodManager imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
				imm.showSoftInput(edsearch, InputMethodManager.SHOW_IMPLICIT);
			}
		});

		edsearch.setOnKeyListener(new OnKeyListener() {

			@Override
			public boolean onKey(View v, int keyCode, KeyEvent event) {
				// TODO Auto-generated method stub
				if (event.getAction() == KeyEvent.ACTION_DOWN) {
					switch (keyCode) {
					case KeyEvent.KEYCODE_DPAD_CENTER:
					case KeyEvent.KEYCODE_ENTER:
						searchtext = edsearch.getText().toString();
						new StoreSearchTask().execute();
						return true;
					default:
						break;
					}
				}
				return false;
			}
		});
		imgsearchbtn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				searchtext = edsearch.getText().toString();
				new StoreSearchTask().execute();
			}
		});
	}

	@Override
	public void onClick(View view) {
		// TODO Auto-generated method stub
		switch (view.getId()) {
		case R.id.btn_by_distance:

			storelist = sortItems(storelist, SortBy.DISTANCE);
			adapter.notifyDataSetChanged();
			slide_me.toggleRightDrawer();
			break;
		case R.id.btn_by_price:
			storelist = sortItems(storelist, SortBy.PRICE);
			adapter.notifyDataSetChanged();
			slide_me.toggleRightDrawer();
			break;
		default:
			break;
		}

	}

	private class GetStoreJSONTask extends AsyncTask<Void, Void, Void> {

		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			pDialog = new ProgressDialog(StorelistActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance

			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.itemstorelisturl + rid,
					ServiceHandler.GET);
			// String jsonStr = sh.makeServiceCall(AppUtility.productlisturl,
			// ServiceHandler.GET);

			Log.e("state Response: ", "> " + jsonStr);

			// http://ec2-54-69-181-234.us-west-2.compute.amazonaws.com:8080/DealSheelAppServer/api/Store?ItemDescriptionId=68427a02-7905-44e2-9f1f-0f3778680a60
			Set<String> uniqueStoreIDs = new HashSet<String>();

			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);
					// looping through All Contacts
					for (int i = 0; i < contacts.length(); i++) {
						uniqueStoreIDs.add(contacts.getJSONObject(i)
								.getJSONObject("objStoreDescription")
								.getString("id"));
						JSONObject c = contacts.getJSONObject(i);
						JSONObject c1 = c.getJSONObject("objStore");
						String id = c1.getString("id");
						String stime = c1.getString("lastUpdated");
						String slat = c1.getString("Lattitude");
						String slong = c1.getString("Longitude");
						String saddress = c1.getString("Address");
						String sphone = c1.getString("PhoneNumber");
						String spostcode = c1.getString("Pincode");

						JSONObject c2 = c.getJSONObject("objStoreDescription");

						String sname = c2.getString("Name");
						String storeid = c2.getString("id");						
						JSONObject c3 = c.getJSONObject("objItem");

						String itemid = c3.getString("id");
						String itemtime = c3.getString("lastUpdated");
						String offerdesc = c3.getString("OfferDescription");
						String itemprice = c3.getString("Price");

						double dis1 = distance(Double.parseDouble(latitude),
								Double.parseDouble(longitude),
								Double.parseDouble(slat),
								Double.parseDouble(slong));
						int dis = (int) dis1;
						HashMap<String, String> map = new HashMap<String, String>();
						map.put("id", id);
						map.put("storetime", stime);
						map.put("lat", slat);
						map.put("long", slong);
						map.put("distance", String.valueOf(dis));
						map.put("address", saddress);

						map.put("phone", sphone);
						map.put("pincode", spostcode);
						map.put("name", sname);
						map.put("storeid", storeid);

						map.put("itemid", itemid);
						map.put("itemtime", itemtime);
						map.put("desc", offerdesc);
						map.put("price", itemprice);

						storelist.add(map);
					}
					Log.d("stores", String.valueOf(uniqueStoreIDs.size()));
				} catch (JSONException e) {
					e.printStackTrace();
				}
			} else {
				Log.e("ServiceHandler", "Couldn't get any data from the url");

			}

			// getfilterdata();

			return null;
		}

		@Override
		protected void onPostExecute(Void result) {
			super.onPostExecute(result);

			if (pDialog.isShowing())
				pDialog.dismiss();

			adapter = new ItemStoreadapter(StorelistActivity.this, storelist);
			storeListView.setAdapter(adapter);
			//
			// right_adapter = new RightMenuAdapter(StorelistActivity.this,
			// rightmenulist);
			// lv_rightmenu.setAdapter(right_adapter);

			// lv.setOnItemClickListener(new OnItemClickListener() {
			//
			// @Override
			// public void onItemClick(AdapterView<?> parent, View view,
			// int position, long id) {
			// // TODO Auto-generated method stub
			// String tempurl = storelist.get(position).get("storeid");
			// Intent i = new Intent(getApplicationContext(),
			// StoreMapActivity.class);
			// i.putExtra("storeid", storelist.get(position)
			// .get("storeid"));
			// i.putExtra("storename", storelist.get(position).get("name"));
			// i.putExtra("storeimage", apputil.topstoreimagesurl
			// + tempurl + ".jpg");
			// Log.e("image send", apputil.topstoreimagesurl + tempurl
			// + ".jpg");
			// i.putExtra("storelat", storelist.get(position).get("lat"));
			// i.putExtra("storelong", storelist.get(position).get("long"));
			// i.putExtra("storephone",
			// storelist.get(position).get("phone"));
			// i.putExtra("storeaddress",
			// storelist.get(position).get("address"));
			// startActivity(i);
			// }
			// });
		}

	}

	private class StoreSearchTask extends AsyncTask<Void, Void, Void> {

		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			pDialog = new ProgressDialog(StorelistActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance
			storelist.clear();
			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.itemstorelistsearchurl
					+ rid + "&SearchText=" + searchtext, ServiceHandler.GET);
			// String jsonStr = sh.makeServiceCall(AppUtility.productlisturl,
			// ServiceHandler.GET);

			Log.e("state Response: ", "> " + jsonStr);

			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);

					// looping through All Contacts
					for (int i = 0; i < contacts.length(); i++) {
						JSONObject c = contacts.getJSONObject(i);
						JSONObject c1 = c.getJSONObject("objStore");
						String id = c1.getString("id");
						String stime = c1.getString("lastUpdated");
						String slat = c1.getString("Lattitude");
						String slong = c1.getString("Longitude");
						String saddress = c1.getString("Address");
						String sphone = c1.getString("PhoneNumber");
						String spostcode = c1.getString("Pincode");

						JSONObject c2 = c.getJSONObject("objStoreDescription");

						String sname = c2.getString("Name");
						String storeid = c2.getString("id");

						JSONObject c3 = c.getJSONObject("objItem");

						String itemid = c3.getString("id");
						String itemtime = c3.getString("lastUpdated");
						String offerdesc = c3.getString("OfferDescription");
						String itemprice = c3.getString("Price");

						double dis1 = distance(Double.parseDouble(latitude),
								Double.parseDouble(longitude),
								Double.parseDouble(slat),
								Double.parseDouble(slong));
						int dis = (int) dis1;
						HashMap<String, String> map = new HashMap<String, String>();
						map.put("id", id);
						map.put("storetime", stime);
						map.put("lat", slat);
						map.put("long", slong);
						map.put("distance", String.valueOf(dis));
						map.put("address", saddress);

						map.put("phone", sphone);
						map.put("pincode", spostcode);
						map.put("name", sname);
						map.put("storeid", storeid);

						map.put("itemid", itemid);
						map.put("itemtime", itemtime);
						map.put("desc", offerdesc);
						map.put("price", itemprice);

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

			adapter = new ItemStoreadapter(StorelistActivity.this, storelist);
			storeListView.setAdapter(adapter);
			storeListView.setEmptyView(findViewById(R.id.emptyview));
			adapter.notifyDataSetChanged();
			lnvsearched.setVisibility(View.GONE);
			imgsearch.setVisibility(View.VISIBLE);
			tvheadertitle.setVisibility(View.VISIBLE);
			imgsearchbtn.setVisibility(View.GONE);
		}

	}

	private double distance(double lat1, double lon1, double lat2, double lon2) {

		double theta = lon1 - lon2;
		double dist = Math.sin(deg2rad(lat1)) * Math.sin(deg2rad(lat2))
				+ Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2))
				* Math.cos(deg2rad(theta));

		dist = Math.acos(dist);

		dist = rad2deg(dist);

		dist = dist * 60 * 1.1515;

		dist = dist * 1.609344;

		return (dist);

	}

	private double deg2rad(double deg) {
		return (deg * Math.PI / 180.0);
	}

	private double rad2deg(double rad) {
		return (rad * 180 / Math.PI);
	}

	private ArrayList<HashMap<String, String>> sortItems(
			ArrayList<HashMap<String, String>> items, SortBy sortBy) {

		switch (sortBy) {
		case PRICE:
			Collections.sort(items, new HashMapArrayComparator("price"));
			break;
		case DISTANCE:
			Collections.sort(items, new HashMapArrayComparator("distance"));
			break;
		default:

			break;
		}
		return items;
	}

	class HashMapArrayComparator implements Comparator<HashMap<String, String>> {

		private String keyToCompare;

		public HashMapArrayComparator(String keyToCompare) {
			this.keyToCompare = keyToCompare;

		}

		@Override
		public int compare(HashMap<String, String> lhs,
				HashMap<String, String> rhs) {
			// TODO Auto-generated method stub
			String leftValue = lhs.get(keyToCompare);
			String rigntValue = rhs.get(keyToCompare);
			return leftValue.compareTo(rigntValue);
		}

	}

	// public void getfilterdata() {
	// ServiceHandler sh = new ServiceHandler();
	// String jsonStr = sh.makeServiceCall(apputil.filtersubcaturl + rid,
	// ServiceHandler.GET);
	// // String jsonStr = sh.makeServiceCall(AppUtility.productlisturl,
	// // ServiceHandler.GET);
	//
	// Log.e("state Response: ", "> " + jsonStr);
	//
	// if (jsonStr != null) {
	// try {
	// JSONArray contacts = new JSONArray(jsonStr);
	//
	// // looping through All Contacts
	// for (int i = 0; i < contacts.length(); i++) {
	// JSONObject c = contacts.getJSONObject(i);
	// String sid = c.getString("id");
	// String sname = c.getString("Name");
	// String model = c.getString("NameAlias");
	// boolean price = c.getBoolean("IsInt");
	// String t = "1";
	// if (price) {
	// t = "1";
	// } else {
	// t = "0";
	// }
	// HashMap<String, String> map = new HashMap<String, String>();
	// map.put("id", sid);
	// map.put("Name", sname);
	// map.put("alias", model);
	// map.put("isint", t);
	//
	// rightmenulist.add(map);
	// }
	// } catch (JSONException e) {
	// e.printStackTrace();
	// }
	// } else {
	// Log.e("ServiceHandler", "Couldn't get any data from the url");
	// }
	// }

	// private class Getfiltervalues extends AsyncTask<Void, Void, Void> {
	//
	// String sid, sname, sctime, slupdate, slat, slong;
	// ArrayList<String> imgPaths = new ArrayList<String>();
	//
	// @Override
	// protected void onPreExecute() {
	// super.onPreExecute();
	// // Showing progress dialog
	// pDialog = new ProgressDialog(StorelistActivity.this);
	// pDialog.setMessage("Please wait...");
	// pDialog.setCancelable(false);
	// pDialog.show();
	//
	// }
	//
	// @Override
	// protected Void doInBackground(Void... arg0) {
	// // Creating service handler class instance
	// boolean tmbool = false;
	// if (getfilterIsint.equalsIgnoreCase("1")) {
	// tmbool = true;
	// } else if (getfilterIsint.equalsIgnoreCase("0")) {
	// tmbool = false;
	// }
	//
	// ServiceHandler sh = new ServiceHandler();
	// String jsonStr = sh.makeServiceCall(apputil.getfiltervaluesurl
	// + rid + "&Name=" + getfilteralias + "&isInt=" + tmbool,
	// ServiceHandler.GET);
	// // String jsonStr = sh.makeServiceCall(AppUtility.productlisturl,
	// // ServiceHandler.GET);
	//
	// Log.e("get filter data Response: ", "> " + jsonStr);
	//
	// if (jsonStr != null) {
	// try {
	// JSONArray contacts = new JSONArray(jsonStr);
	//
	// // looping through All Contacts
	// for (int i = 0; i < contacts.length(); i++) {
	// JSONObject c = contacts.getJSONObject(i);
	// sid = c.toString();
	// imgPaths.add(sid);
	// }
	// } catch (JSONException e) {
	// e.printStackTrace();
	// }
	// } else {
	// Log.e("ServiceHandler", "Couldn't get any data from the url");
	// }
	//
	// return null;
	// }
	//
	// @SuppressLint("NewApi")
	// @Override
	// protected void onPostExecute(Void result) {
	// super.onPostExecute(result);
	//
	// if (pDialog.isShowing())
	// pDialog.dismiss();
	//
	// AlertDialog.Builder alertDialog = new AlertDialog.Builder(
	// StorelistActivity.this, AlertDialog.THEME_HOLO_LIGHT);
	// LayoutInflater inflater = getLayoutInflater();
	// View convertView = (View) inflater.inflate(R.layout.dialoglist,
	// null);
	// alertDialog.setView(convertView);
	// alertDialog.setTitle("Select");
	// ListView lv = (ListView) convertView.findViewById(R.id.lv_dialog);
	// ArrayAdapter<String> adapter = new ArrayAdapter<String>(
	// StorelistActivity.this,
	// android.R.layout.simple_list_item_1, imgPaths);
	// lv.setAdapter(adapter);
	// alertDialog.show();
	//
	// lv.setOnItemClickListener(new OnItemClickListener() {
	//
	// @Override
	// public void onItemClick(AdapterView<?> parent, View view,
	// int position, long id) {
	// // TODO Auto-generated method stub
	//
	// }
	// });
	// }
	//
	// }
	//
	// private class Applyfiltervalues extends AsyncTask<Void, Void, Void> {
	//
	// String sid, sname, sctime, slupdate, slat, slong;
	//
	// @Override
	// protected void onPreExecute() {
	// super.onPreExecute();
	// // Showing progress dialog
	// pDialog = new ProgressDialog(StorelistActivity.this);
	// pDialog.setMessage("Please wait...");
	// pDialog.setCancelable(false);
	// pDialog.show();
	//
	// }
	//
	// @Override
	// protected Void doInBackground(Void... arg0) {
	// // Creating service handler class instance
	// boolean tmbool = false;
	// if (getfilterIsint.equalsIgnoreCase("1")) {
	// tmbool = true;
	// } else if (getfilterIsint.equalsIgnoreCase("0")) {
	// tmbool = false;
	// }
	//
	// return null;
	// }
	//
	// @Override
	// protected void onPostExecute(Void result) {
	// super.onPostExecute(result);
	//
	// if (pDialog.isShowing())
	// pDialog.dismiss();
	//
	// }
	//
	// }
	//
	// PanelSlideListener slideListener = new PanelSlideListener() {
	//
	// @Override
	// public void onPanelSlide(View arg0, float arg1) {
	// Log.d("d", "slide");
	//
	// }
	//
	// @Override
	// public void onPanelOpened(View arg0) {
	//
	// }
	//
	// @Override
	// public void onPanelClosed(View arg0) {
	//
	// Log.d("d", "close");
	// }
	// };

	// private void createStoreGroups(String jSONString) {
	// try {
	//
	// Set<String> uniqueStoreIDs = new HashSet<String>();
	// JSONArray storesArray = new JSONArray(jSONString);
	// for (int index = 0; index < storesArray.length(); index++) {
	// JSONObject avalibableDescriptoin = storesArray
	// .getJSONObject(index);
	// JSONObject storeDescription = avalibableDescriptoin
	// .getJSONObject("objStoreDescrition");
	// uniqueStoreIDs.add(storeDescription.getString("id"));
	// }
	//
	// for (int i = 0; i < storesArray.length(); i++) {
	// JSONObject c = storesArray.getJSONObject(i);
	// JSONObject c1 = c.getJSONObject("objStore");
	// String id = c1.getString("id");
	// String stime = c1.getString("lastUpdated");
	// String slat = c1.getString("Lattitude");
	// String slong = c1.getString("Longitude");
	// String saddress = c1.getString("Address");
	// String sphone = c1.getString("PhoneNumber");
	// String spostcode = c1.getString("Pincode");
	//
	// JSONObject c2 = c.getJSONObject("objStoreDescription");
	//
	// String sname = c2.getString("Name");
	// String storeid = c2.getString("id");
	//
	// JSONObject c3 = c.getJSONObject("objItem");
	//
	// String itemid = c3.getString("id");
	// String itemtime = c3.getString("lastUpdated");
	// String offerdesc = c3.getString("OfferDescription");
	// String itemprice = c3.getString("Price");
	//
	// double dis1 = distance(Double.parseDouble(latitude),
	// Double.parseDouble(longitude),
	// Double.parseDouble(slat), Double.parseDouble(slong));
	// int dis = (int) dis1;
	// HashMap<String, String> map = new HashMap<String, String>();
	// map.put("id", id);
	// map.put("storetime", stime);
	// map.put("lat", slat);
	// map.put("long", slong);
	// map.put("distance", String.valueOf(dis));
	// map.put("address", saddress);
	//
	// map.put("phone", sphone);
	// map.put("pincode", spostcode);
	// map.put("name", sname);
	// map.put("storeid", storeid);
	//
	// map.put("itemid", itemid);
	// map.put("itemtime", itemtime);
	// map.put("desc", offerdesc);
	// map.put("price", itemprice);
	//
	// storelist.add(map);
	// }
	// } catch (JSONException e) {
	// e.printStackTrace();
	// }
	//
	// }

}
