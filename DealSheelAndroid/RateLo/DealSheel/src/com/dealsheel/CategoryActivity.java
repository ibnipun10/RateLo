package com.dealsheel;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
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
import android.os.StrictMode;
import android.support.v4.widget.SlidingPaneLayout.PanelSlideListener;
import android.util.Log;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnKeyListener;
import android.view.inputmethod.InputMethodManager;
import android.widget.AbsListView;
import android.widget.AbsListView.OnScrollListener;
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

import com.lazylist.Categoryadapter;
import com.lazylist.Catlistviewadapter;
import com.lazylist.RightMenuAdapter;
import com.lazylist.SubCategoryadapter;
import com.meetme.android.horizontallistview.HorizontalListView;
import com.navdrawer.SimpleSideDrawer;
import com.utility.AppUtility;
import com.utility.CategoryHelper;
import com.utility.ServiceHandler;

public class CategoryActivity extends Activity {

	private HorizontalListView lvmaincat,lvsubcat;
	ArrayList<HashMap<String, String>> catlist,subcatlist;
	ArrayList<HashMap<String,String>> listviewlist;
	AppUtility apputil;
	TextView tvmaincat,tvsubcat;
	ListView lvlist;
	CategoryHelper helper;
	Categoryadapter catadapter;
	SubCategoryadapter subcatadapter;
	Catlistviewadapter listviewadapter;
	String stateid,subcatid,rmaincatid,rsubcatname,rmaincatname,searchtext;
	private ProgressDialog pDialog;
	ImageView imgarrow,imgsetting,imgsearch,imgsearchbtn,imgfilter,imgrightclose,imgleftclose;
	Boolean searchflag=false;
	LinearLayout lnvseach,lnvsearched;
	Button btnclear;
	EditText edsearch;
	SimpleSideDrawer slide_me;
	ExpandableListView lv_rightmenu;
	RightMenuAdapter right_adapter;
	ArrayList<HashMap<String,String>> rightmenulist;
	String getfilterIsint,getfilteralias;
	Button buttonApplyFilter;
	private boolean hasMoreItems=true;
	
	
	@SuppressLint("NewApi")
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.category);
		
		if (android.os.Build.VERSION.SDK_INT > 9) {
		    StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
		    StrictMode.setThreadPolicy(policy);
		}
		listviewlist=new ArrayList<HashMap<String,String>>();
		rightmenulist=new ArrayList<HashMap<String,String>>();
		edsearch=(EditText)findViewById(R.id.ed_filtercategory_search);
		tvmaincat=(TextView)findViewById(R.id.tv_category_maincat);
		tvsubcat=(TextView)findViewById(R.id.tv_category_subcat);
		lvmaincat=(HorizontalListView)findViewById(R.id.lv_category_maincat);
		lvsubcat=(HorizontalListView)findViewById(R.id.lv_category_subcat);
		lvlist=(ListView)findViewById(R.id.lv_category);
		imgarrow=(ImageView)findViewById(R.id.img_category_downarrow);
		imgsetting=(ImageView)findViewById(R.id.btn_category_left);
		imgsearch=(ImageView)findViewById(R.id.img_category_search);
		imgsearchbtn=(ImageView)findViewById(R.id.img_filtercategory_searchbtn);
		imgfilter=(ImageView)findViewById(R.id.img_category_filter);
		
		lnvseach=(LinearLayout)findViewById(R.id.lnv_filtercategory_searchbar);
		lnvsearched=(LinearLayout)findViewById(R.id.lnv_searchbar_edittext);
		btnclear=(Button)findViewById(R.id.btn_search_cleartext);
		 
		btnclear.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				edsearch.setText("");
			}
		});
		apputil=new AppUtility(CategoryActivity.this);
		catlist=new ArrayList<HashMap<String,String>>();
		subcatlist=new ArrayList<HashMap<String,String>>();
		
		slide_me = new SimpleSideDrawer(this);
		slide_me.setLeftBehindContentView(R.layout.leftpanel);
		slide_me.setRightBehindContentView(R.layout.right_menu);
		
		lv_rightmenu=(ExpandableListView)findViewById(R.id.elv_rightmenu);
		imgleftclose=(ImageView)findViewById(R.id.img_leftmenu_close);
		imgrightclose=(ImageView)findViewById(R.id.img_rightmenu_close);
		buttonApplyFilter=(Button)findViewById(R.id.btn_apply_filter);
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
		helper=new CategoryHelper(CategoryActivity.this);
		
		Intent i = getIntent();
		rmaincatid=i.getStringExtra("mid");
		subcatid=i.getStringExtra("sid");
		rmaincatname=i.getStringExtra("mname");
		rsubcatname=i.getStringExtra("sname");
		tvmaincat.setText(rmaincatname);
		tvsubcat.setText(rsubcatname);
		
	
		
		stateid=apputil.getstateid();
		new GetContacts().execute();
		
		
		
		
		lvlist.setOnScrollListener(new OnScrollListener() {
			
			@Override
			public void onScrollStateChanged(AbsListView view, int scrollState) {
				// TODO Auto-generated method stub
				
			}
			
			@Override
			public void onScroll(AbsListView view, int firstVisibleItem,
					int visibleItemCount, int totalItemCount) {
				// TODO Auto-generated method stub
				
				boolean loadMore=firstVisibleItem+visibleItemCount>=totalItemCount;
				if(loadMore&&hasMoreItems){
					new LoadMoreItems().execute(String.valueOf(totalItemCount));
				}
				
			}
		});
		
		/*lvmaincat.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				subcatlist=getsubcategory(catlist.get(position).get("id"));
				subcatadapter=new SubCategoryadapter(CategoryActivity.this, subcatlist);
				lvsubcat.setAdapter(subcatadapter);
				
				tvmaincat.setText(catlist.get(position).get("name"));
				tvsubcat.setText(subcatlist.get(0).get("name"));
				
				subcatid=subcatlist.get(0).get("id");
				new GetContacts().execute();
			}
		});
		
		lvsubcat.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				
				tvsubcat.setText(subcatlist.get(position).get("name"));
				subcatid=subcatlist.get(position).get("id");
				new GetContacts().execute();
			}
		});*/
		
		lvlist.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				
				Intent i=new Intent(CategoryActivity.this, StorelistActivity.class);
				i.putExtra("id", listviewlist.get(position).get("id"));
				i.putExtra("flag", 1);
				startActivity(i);
			}
		});
		
		
buttonApplyFilter.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
			new Applyfiltervalues().execute();			
			}
		});
		
		
		lv_rightmenu.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				
				getfilteralias=rightmenulist.get(position).get("alias");
				getfilterIsint=rightmenulist.get(position).get("isint");
				new Getfiltervalues().execute();
			}
		});
		
		imgarrow.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				finish();
			}
		});
		
		TextView tvs=(TextView)findViewById(R.id.tv_leftpanel_setting);
		tvs.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				slide_me.toggleLeftDrawer();
				
				startActivity(new Intent(getApplicationContext(),SettingActivity.class));
			}
		});
		
		TextView tvrateus=(TextView)findViewById(R.id.tv_leftpanel_Rateus);
		tvrateus.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://play.google.com/store/apps/details?id=com.dealsheel"));
				startActivity(browserIntent);
			}
		});
		
		TextView tvcontactus=(TextView)findViewById(R.id.tv_leftpanel_contact);
		tvcontactus.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("http://www.ratelo.com:8080/DealSheelAppServer/"));
				startActivity(browserIntent);
			}
		});
		
		imgfilter.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				slide_me.toggleRightDrawer();
				/**/
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
		
		imgsearch.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				
					lnvseach.setVisibility(View.GONE);
					imgarrow.setVisibility(View.GONE);
					lnvsearched.setVisibility(View.VISIBLE);
					imgsearch.setVisibility(View.GONE);
					imgsearchbtn.setVisibility(View.VISIBLE);
					edsearch.requestFocus();
					InputMethodManager imm=(InputMethodManager)getSystemService(Context.INPUT_METHOD_SERVICE);
					imm.showSoftInput(edsearch, InputMethodManager.SHOW_IMPLICIT);
			}
		});
		
		edsearch.setOnKeyListener(new OnKeyListener() {
			
			@Override
			public boolean onKey(View v, int keyCode, KeyEvent event) {
				// TODO Auto-generated method stub
				if(event.getAction() == KeyEvent.ACTION_DOWN)
				{
					switch (keyCode)
					{
					case KeyEvent.KEYCODE_DPAD_CENTER:
					case KeyEvent.KEYCODE_ENTER:	
						searchtext=edsearch.getText().toString();
						new GetContacts1().execute();
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
				
				searchtext=edsearch.getText().toString();
				new GetContacts1().execute();
				
				/*lnvseach.setVisibility(View.VISIBLE);
				imgarrow.setVisibility(View.VISIBLE);
				edsearch.setVisibility(View.GONE);
				imgsearch.setVisibility(View.VISIBLE);
				imgsearchbtn.setVisibility(View.GONE);*/
			}
		});
		
	}
	
	private class GetContacts extends AsyncTask<Void, Void, Void> {

		String sid,sname,sctime,slupdate,slat,slong;
		
		
		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			listviewlist.clear();
			pDialog = new ProgressDialog(CategoryActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance
			
			listviewlist.clear();
			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.itemfromsubcatlisturl+subcatid+"&Stateid="+stateid, ServiceHandler.GET);
			//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
			
			Log.e("state Response: ", "> " + jsonStr);

			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);

					// looping through All Contacts
					for (int i = 0; i <contacts.length() ; i++) {
						JSONObject c = contacts.getJSONObject(i);
						sid = c.getString("id");
						sname = c.getString("Brand");
						String model=c.getString("ModelNumber");
						String price=c.getString("MinPrice");
						
						HashMap<String, String> map=new HashMap<String, String>();
						map.put("id", sid);
						map.put("Name", sname);
						map.put("model", model);
						map.put("price", price);
						
						listviewlist.add(map);
					}
				} catch (JSONException e) {
					e.printStackTrace();
				}
			} else {
				Log.e("ServiceHandler", "Couldn't get any data from the url");
			}
			
			getfilterdata();
			
			return null;
		}

		@Override
		protected void onPostExecute(Void result) {
			super.onPostExecute(result);
						
			
			 if (pDialog.isShowing())
					pDialog.dismiss();
			 
			 listviewadapter=new Catlistviewadapter(CategoryActivity.this, listviewlist);
			 lvlist.setAdapter(listviewadapter);
			 listviewadapter.notifyDataSetChanged();
			 
			 right_adapter=new RightMenuAdapter(CategoryActivity.this, rightmenulist);
			 lv_rightmenu.setAdapter(right_adapter);
			 slide_me.closeRightSide();
		}

	}
	
	public ArrayList<HashMap<String, String>> getsubcategory(String cid) 
	{
		ArrayList<HashMap<String, String>> templist=new ArrayList<HashMap<String,String>>();
		templist=helper.getsubcat(cid);
		
		return templist;
	}
	
	public void getfilterdata() 
	{
		ServiceHandler sh = new ServiceHandler();
		String jsonStr = sh.makeServiceCall(apputil.filtersubcaturl+subcatid, ServiceHandler.GET);		
		//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
		rightmenulist.clear();
		Log.e("state Response: ", "> " + jsonStr);

		if (jsonStr != null) {
			try {
				JSONArray contacts = new JSONArray(jsonStr);

				// looping through All Contacts
				for (int i = 0; i <contacts.length() ; i++) {
					JSONObject c = contacts.getJSONObject(i);
					String sid = c.getString("id");
					String sname = c.getString("Name");
					String model=c.getString("NameAlias");
					boolean isInt=c.getBoolean("IsInt");
					String t="1";
					if(isInt)
					{
						t="1";
					}
					else
					{
						t="0";
					}
					HashMap<String, String> map=new HashMap<String, String>();
					map.put("id", sid);
					map.put("Name", sname);
					map.put("alias", model);
					map.put("isint", t);

					String url=apputil.getfiltervaluesurl+subcatid+"&Name="+sname.replaceAll(" ","_")+"&isInt="+String.valueOf(isInt);
					String filterValues=sh.makeServiceCall(url,ServiceHandler.GET);
					JSONArray subFilters=new JSONArray(filterValues);
					map.put("subsize", String.valueOf(subFilters.length()));					
					for(int index=0;index<subFilters.length();index++){
						map.put(String.valueOf(index), subFilters.getString(index));						
					}				
								
					rightmenulist.add(map);
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		} else {
			Log.e("ServiceHandler", "Couldn't get any data from the url");
		}	
	}

	private class GetContacts1 extends AsyncTask<Void, Void, Void> {

		String sid,sname,sctime,slupdate,slat,slong;
		
		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			pDialog = new ProgressDialog(CategoryActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance
			
			listviewlist.clear();
			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.itemsearchurl+stateid+"&SearchText="+searchtext, ServiceHandler.GET);
			//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
			
			Log.e("state Response: ", "> " + jsonStr);

			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);

					// looping through All Contacts
					for (int i = 0; i <contacts.length() ; i++) {
						JSONObject c = contacts.getJSONObject(i);
						sid = c.getString("id");
						sname = c.getString("Brand");
						String model=c.getString("ModelNumber");
						String price=c.getString("MinPrice");
						
						HashMap<String, String> map=new HashMap<String, String>();
						map.put("id", sid);
						map.put("Name", sname);
						map.put("model", model);
						map.put("price", price);
						
						listviewlist.add(map);
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
			 
			 listviewadapter=new Catlistviewadapter(CategoryActivity.this, listviewlist);
			 lvlist.setAdapter(listviewadapter);
			 lvlist.setEmptyView(findViewById(R.id.emptyview));
			 listviewadapter.notifyDataSetChanged();
			 
			 lnvseach.setVisibility(View.VISIBLE);
				imgarrow.setVisibility(View.VISIBLE);
				lnvsearched.setVisibility(View.GONE);
				imgsearch.setVisibility(View.VISIBLE);
				imgsearchbtn.setVisibility(View.GONE);
				
		}

	}
	
	private class Getfiltervalues extends AsyncTask<Void, Void, Void> {

		String sid,sname,sctime,slupdate,slat,slong;
		ArrayList<String> imgPaths = new ArrayList<String>();
		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			pDialog = new ProgressDialog(CategoryActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance
			boolean tmbool = false;
			if(getfilterIsint.equalsIgnoreCase("1"))
			{
				tmbool=true;
			}else if(getfilterIsint.equalsIgnoreCase("0"))
			{
				tmbool=false;
			}
			
			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.getfiltervaluesurl+subcatid+"&Name="+getfilteralias+"&isInt="+tmbool, ServiceHandler.GET);
			//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
			
			Log.e("get filter data Response: ", "> " + jsonStr);

			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);

					// looping through All Contacts
					for (int i = 0; i <contacts.length() ; i++) {
						JSONObject c = contacts.getJSONObject(i);
						sid = c.toString();
						imgPaths.add(sid);
					}
				} catch (JSONException e) {
					e.printStackTrace();
				}
			} else {
				Log.e("ServiceHandler", "Couldn't get any data from the url");
			}
			
			return null;
		}

		@SuppressLint("NewApi")
		@Override
		protected void onPostExecute(Void result) {
			super.onPostExecute(result);
						
			
			 if (pDialog.isShowing())
					pDialog.dismiss();
			 
			 AlertDialog.Builder alertDialog = new AlertDialog.Builder(CategoryActivity.this,AlertDialog.THEME_HOLO_LIGHT);
		        LayoutInflater inflater = getLayoutInflater();
		        View convertView = (View) inflater.inflate(R.layout.dialoglist, null);
		        alertDialog.setView(convertView);
		        alertDialog.setTitle("Select");
		        ListView lv = (ListView) convertView.findViewById(R.id.lv_dialog);
		        ArrayAdapter<String> adapter = new ArrayAdapter<String>(CategoryActivity.this,android.R.layout.simple_list_item_1,imgPaths);
		        lv.setAdapter(adapter);
		        alertDialog.show();
		        
		        lv.setOnItemClickListener(new OnItemClickListener() {

					@Override
					public void onItemClick(AdapterView<?> parent, View view,
							int position, long id) {
						// TODO Auto-generated method stub
						
						
					}
				});
		}

	}
	
	private class Applyfiltervalues extends AsyncTask<Void, Void, Void> {

		String sid,sname,sctime,slupdate,slat,slong;		
		HashMap<String, List<String>> filterValues;
		Boolean emptyFilter=false;
		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			pDialog = new ProgressDialog(CategoryActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance			
			
			filterValues=right_adapter.getFilterValues();
//			boolean tmbool = false;
//			if(getfilterIsint.equalsIgnoreCase("1"))
//			{
//				tmbool=true;
//			}else if(getfilterIsint.equalsIgnoreCase("0"))
//			{
//				tmbool=false;
//			}
							
				
			List<NameValuePair> attributes= new ArrayList<NameValuePair>();
			BasicNameValuePair subcatarg=new BasicNameValuePair("SubCategoryId", subcatid);
			BasicNameValuePair statearg= new BasicNameValuePair("Stateid",stateid);
			attributes.add(subcatarg);
			attributes.add(statearg);						
			for(int i=0;i<rightmenulist.size();i++)
			{
				String attributeKey=rightmenulist.get(i).get("Name");
				List<String> filters=filterValues.get(attributeKey);
				String filterList ="";
				if(filters.size()>0){					
					if(rightmenulist.get(i).get("isint")!="1"){
				for(int j=0;j<filters.size();j++){
					if(filters.get(j)!="null" &&filters.get(j).length()>0){
					if(j==0)
					{
					filterList=filters.get(j).trim();
					}
					else
					{
					filterList=filterList+","+filters.get(j).trim();
					}
					}	
				}
				}				
				if(rightmenulist.get(i).get("isint")=="1"){
				filterList="0-"+filters.get(0);	
				}
				BasicNameValuePair filterName= new BasicNameValuePair("Name",attributeKey.replaceAll(" ","_"));
				BasicNameValuePair filterValue=new BasicNameValuePair("Value", filterList.replaceAll(" ","%20"));
				BasicNameValuePair isInt= new BasicNameValuePair("isInt",(rightmenulist.get(i).get("isint")=="1"?"true":"false"));
				attributes.add(filterName);
				attributes.add(filterValue);
				attributes.add(isInt);
				}
			}
			
			if(attributes.size()<=2){
				emptyFilter=true;
				return null;
			}
			else{
				emptyFilter=false;
			}
			ServiceHandler sh = new ServiceHandler();			
			String jsonStr = sh.makeServiceCall(AppUtility.applyfiltersubcaturlnoattribs, ServiceHandler.GET,attributes,true);
			if(jsonStr!=null);
			listviewlist.clear();
			//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);			
			Log.e("get filter data Response: ", "> " + jsonStr);
			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);

					// looping through All Contacts
					for (int i = 0; i <contacts.length() ; i++) {
						JSONObject c = contacts.getJSONObject(i);
						sid = c.getString("id");
						sname = c.getString("Brand");
						String model=c.getString("ModelNumber");
						String price=c.getString("MinPrice");						
						HashMap<String, String> map=new HashMap<String, String>();
						map.put("id", sid);
						map.put("Name", sname);
						map.put("model", model);
						map.put("price", price);						
						listviewlist.add(map);
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
			 if(!emptyFilter){
			 listviewadapter.notifyDataSetChanged();
			 slide_me.closeRightSide();
			 }
			 else{
				 new GetContacts().execute();
			 }
			 
			
			 
		}

	}
	
	
	
	private class LoadMoreItems extends AsyncTask<String, Void, Void> {

		String sid,sname,sctime,slupdate,slat,slong;
		private String skipUpto;
		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog		
		

		}

		@Override
		protected Void doInBackground(String... arg) {
			// Creating service handler class instance		
			skipUpto=arg[0];
			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.itemfromsubcatlisturl+subcatid+"&Stateid="+stateid+"&Skip="+skipUpto, ServiceHandler.GET);
			if (jsonStr != null) {
				try {
					JSONArray items = new JSONArray(jsonStr);
					
					for (int i = 0; i <items.length() ; i++) {
						JSONObject c = items.getJSONObject(i);
						sid = c.getString("id");
						sname = c.getString("Brand");
						String model=c.getString("ModelNumber");
						String price=c.getString("MinPrice");
						
						HashMap<String, String> map=new HashMap<String, String>();
						map.put("id", sid);
						map.put("Name", sname);
						map.put("model", model);
						map.put("price", price);						
						listviewlist.add(map);
					}
				} catch (JSONException e) {
					e.printStackTrace();
					hasMoreItems=false;
				}
			} else {
				Log.e("ServiceHandler", "Couldn't get any data from the url");
				hasMoreItems=false;
			}		
			return null;
		}

		@Override
		protected void onPostExecute(Void result) {
			super.onPostExecute(result);			
			 listviewadapter.notifyDataSetChanged();		
		}

	}
	
	
	
	
	
	
	
	
PanelSlideListener slideListener = new PanelSlideListener() {
		
		@Override
		public void onPanelSlide(View arg0, float arg1) {
			
		}
		
		@Override
		public void onPanelOpened(View arg0) {
			
		}
		
		@Override
		public void onPanelClosed(View arg0) {
			
		}
	};
}