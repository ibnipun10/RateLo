package com.dealsheel;

import java.util.ArrayList;
import java.util.HashMap;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import com.lazylist.Categoryadapter;
import com.lazylist.SubCategoryadapter;
import com.lazylist.TopCategoryadapter;
import com.lazylist.Topitemadapter;
import com.lazylist.Topstoreadapter;
import com.meetme.android.horizontallistview.HorizontalListView;
import com.meetme.android.horizontallistview.HorizontalListView.OnScrollStateChangedListener;
import com.navdrawer.SimpleSideDrawer;
import com.utility.AppUtility;
import com.utility.CategoryHelper;
import com.utility.ConnectionDetector;
import com.utility.ServiceHandler;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v4.widget.SlidingPaneLayout;
import android.support.v4.widget.SlidingPaneLayout.PanelSlideListener;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

public class FeaturedActivity extends Activity {

	private HorizontalListView lvtrending,lvpopularstore,lvmostcat,lvmaincat,lvsubcat;
	ArrayList<HashMap<String, String>> trendinglist,storelist,topcatlist;
	ArrayList<HashMap<String, String>> catlist,subcatlist;
	AppUtility apputil;
	private ProgressDialog pDialog;
	TopCategoryadapter catadapter;
	Topitemadapter itemadapter;
	CategoryHelper helper;
	Categoryadapter maincatadapter;
	SubCategoryadapter subcatadapter;
	Topstoreadapter storeadapter;
	ImageView imgdownarrow,imgtrendngleft,imgtrendngright,imgstoreleft,imgstoreright,imgcategoryleft,imgcategoryright,imgrightclose,imgleftclose;
	Button imgsetting;
	ConnectionDetector cd;
	Boolean isInternetPresent = false;
	LinearLayout lnv_mainvat,lnv_subcat;
	Boolean iscat=false;
	String tsmid,tssid,tsmane,tssname;
	SimpleSideDrawer slide_me;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.featured);
		
		lvmostcat=(HorizontalListView)findViewById(R.id.lv_featured_topcategory);
		lvpopularstore=(HorizontalListView)findViewById(R.id.lv_featured_populerstore);
		lvtrending=(HorizontalListView)findViewById(R.id.lv_featured_tending);
		lvmaincat=(HorizontalListView)findViewById(R.id.lv_category_maincat);
		lvsubcat=(HorizontalListView)findViewById(R.id.lv_category_subcat);
		
		imgtrendngright=(ImageView)findViewById(R.id.img_trending_rightarrow);
		imgtrendngleft=(ImageView)findViewById(R.id.img_trending_leftarrow);
		imgstoreright=(ImageView)findViewById(R.id.img_stores_rightarrow);
		imgstoreleft=(ImageView)findViewById(R.id.img_stores_leftarrow);
		imgcategoryright=(ImageView)findViewById(R.id.img_category_rightarrow);
		imgcategoryleft=(ImageView)findViewById(R.id.img_category_leftarrow);
		imgdownarrow=(ImageView)findViewById(R.id.img_featured_downarrow);
		imgsetting=(Button)findViewById(R.id.btn_featured_left);
		
		slide_me = new SimpleSideDrawer(this);
		slide_me.setLeftBehindContentView(R.layout.leftpanel);
		slide_me.setRightBehindContentView(R.layout.right_menu);
		
		imgleftclose=(ImageView)findViewById(R.id.img_leftmenu_close);
		imgrightclose=(ImageView)findViewById(R.id.img_rightmenu_close);
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
		
		lnv_mainvat=(LinearLayout)findViewById(R.id.lnv_category_maincat);
		lnv_subcat=(LinearLayout)findViewById(R.id.lnv_category_subcat);
		
		apputil=new AppUtility(FeaturedActivity.this);
		trendinglist=new ArrayList<HashMap<String,String>>();
		storelist=new ArrayList<HashMap<String,String>>();
		topcatlist=new ArrayList<HashMap<String,String>>();
		catlist=new ArrayList<HashMap<String,String>>();
		subcatlist=new ArrayList<HashMap<String,String>>();
		
		helper=new CategoryHelper(FeaturedActivity.this);
		cd = new ConnectionDetector(getApplicationContext());
		isInternetPresent = cd.isConnectingToInternet();
		 
        // check for Internet status
        if (isInternetPresent) 
        {
        	
        }
        else 
        {
            
            showAlertDialog(FeaturedActivity.this, "No Internet Connection",
                    "You Need Internet Connection To Run This App", false);
        }
        
        
		//lvmaincat.setSelection(0);
		
		
		imgdownarrow.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if(!iscat)
				{
					catlist=helper.getCategorylist();
					maincatadapter=new Categoryadapter(FeaturedActivity.this, catlist,"");
					lvmaincat.setAdapter(maincatadapter);
				lnv_mainvat.setVisibility(View.VISIBLE);
				imgdownarrow.setImageResource(R.drawable.up_arrow);
				iscat=true;
				}
				else
				{
					lnv_mainvat.setVisibility(View.GONE);
					lnv_subcat.setVisibility(View.GONE);
					imgdownarrow.setImageResource(R.drawable.down_arrow);
					iscat=false;
				}
				/*startActivity(new Intent(getApplicationContext(),CategoryActivity.class));*/
			}
		});
		
		
		
		lvmaincat.setOnItemClickListener(new OnItemClickListener() {

			@SuppressLint("ResourceAsColor")
			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				
				maincatadapter=new Categoryadapter(FeaturedActivity.this, catlist,String.valueOf(position));
				lvmaincat.setAdapter(maincatadapter);
				tsmid=catlist.get(position).get("id");
				tsmane=catlist.get(position).get("name");
				lnv_subcat.setVisibility(View.VISIBLE);
				subcatlist=getsubcategory(catlist.get(position).get("id"));
				subcatadapter=new SubCategoryadapter(FeaturedActivity.this, subcatlist);
				lvsubcat.setAdapter(subcatadapter);
			}
		});
		
		lvsubcat.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				
				lnv_subcat.setVisibility(View.GONE);
				lnv_mainvat.setVisibility(View.GONE);
				imgdownarrow.setImageResource(R.drawable.down_arrow);
				
				Intent i=new Intent(getApplicationContext(), CategoryActivity.class);
				i.putExtra("mid", tsmid);
				i.putExtra("sid", subcatlist.get(position).get("id"));
				i.putExtra("mname", tsmane);
				i.putExtra("sname", subcatlist.get(position).get("name"));
				startActivity(i);
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
		
		imgsetting.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				slide_me.toggleLeftDrawer();
				
			}
		});
		new GetContacts().execute();
		
		lvtrending.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				
				Intent i=new Intent(FeaturedActivity.this, StorelistActivity.class);
				i.putExtra("flag", 0);
				i.putExtra("id", trendinglist.get(position).get("id"));
				i.putExtra("image", trendinglist.get(position).get("id"));
				i.putExtra("model", trendinglist.get(position).get("Name"));
				i.putExtra("desc", trendinglist.get(position).get("longdesc"));
				i.putExtra("title", trendinglist.get(position).get("shortdesc"));
				startActivity(i);
			}
		});
		
		lvtrending.setOnScrollStateChangedListener(new OnScrollStateChangedListener() {
			
			@Override
			public void onScrollStateChanged(ScrollState scrollState) {
				// TODO Auto-generated method stub
				int threshold = 1;
                int count = trendinglist.size();
				 Log.e("position", lvtrending.getLastVisiblePosition()+"");
				 if (scrollState == ScrollState.SCROLL_STATE_IDLE)
				 {
	                    if (lvtrending.getLastVisiblePosition() == count-1)
	                    {
	                    	imgtrendngleft.setVisibility(View.VISIBLE);
	                    	imgtrendngright.setVisibility(View.GONE);
	                    }
	                    else if (lvtrending.getLastVisiblePosition() == 2)
	                    {
	                    	imgtrendngleft.setVisibility(View.GONE);
	                    	imgtrendngright.setVisibility(View.VISIBLE);
	                    }
				 }
			}
		});
		
		lvpopularstore.setOnScrollStateChangedListener(new OnScrollStateChangedListener() {
			
			@Override
			public void onScrollStateChanged(ScrollState scrollState) {
				// TODO Auto-generated method stub
				int threshold = 1;
                int count = trendinglist.size();
				 Log.e("position", lvtrending.getLastVisiblePosition()+"");
				 if (scrollState == ScrollState.SCROLL_STATE_IDLE)
				 {
	                    if (lvpopularstore.getLastVisiblePosition() == count-1)
	                    {
	                    	imgstoreleft.setVisibility(View.VISIBLE);
	                    	imgstoreright.setVisibility(View.GONE);
	                    }
	                    else if (lvpopularstore.getLastVisiblePosition() == 2)
	                    {
	                    	imgstoreleft.setVisibility(View.GONE);
	                    	imgstoreright.setVisibility(View.VISIBLE);
	                    }
				 }
			}
		});

	lvmostcat.setOnScrollStateChangedListener(new OnScrollStateChangedListener() {
	
	@Override
	public void onScrollStateChanged(ScrollState scrollState) {
		// TODO Auto-generated method stub
		int threshold = 1;
        int count = trendinglist.size();
		 Log.e("position", lvtrending.getLastVisiblePosition()+"");
		 if (scrollState == ScrollState.SCROLL_STATE_IDLE)
		 {
                if (lvmostcat.getLastVisiblePosition() == count-1)
                {
                	imgcategoryleft.setVisibility(View.VISIBLE);
                	imgcategoryright.setVisibility(View.GONE);
                }
                else if (lvtrending.getLastVisiblePosition() == 2)
                {
                	imgcategoryleft.setVisibility(View.GONE);
                	imgcategoryright.setVisibility(View.VISIBLE);
                }
		 	}
		}
	});
		
		lvmostcat.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				Intent i=new Intent(FeaturedActivity.this, FIlterSubcategoryActivity.class);
				i.putExtra("id", topcatlist.get(position).get("id"));
				i.putExtra("name", topcatlist.get(position).get("Name"));
				
				startActivity(i);
			}
		});
		
		lvpopularstore.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				Intent i=new Intent(FeaturedActivity.this, StoreMapviewActivity.class);
				i.putExtra("storeid", storelist.get(position).get("id"));
				i.putExtra("storename", storelist.get(position).get("Name"));
				i.putExtra("storeimage", apputil.topstoreimagesurl+storelist.get(position).get("id")+".jpg");
				
				startActivity(i);
			}
		});
	}
	
	
	
	public ArrayList<HashMap<String, String>> getsubcategory(String cid) 
	{
		ArrayList<HashMap<String, String>> templist=new ArrayList<HashMap<String,String>>();
		templist=helper.getsubcat(cid);
		
		return templist;
	}
	
	private class GetContacts extends AsyncTask<Void, Void, Void> {

		String sid,sname,sctime,slupdate,slat,longdesc;
		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			pDialog = new ProgressDialog(FeaturedActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance
			
			gettopstore();
			gettopcategory();
			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.topitemsurl, ServiceHandler.GET);
			//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
			
			Log.e("trending Response: ", "> " + jsonStr);

			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);

					// looping through All Contacts
					for (int i = 0; i <contacts.length() ; i++) {
						JSONObject c = contacts.getJSONObject(i);
						JSONObject c1=c.getJSONObject("objclass");
						sid = c1.getString("id");
						sname = c1.getString("Model_Name");
						String mname=c1.getString("Brand");
						String shortdesc=c1.getString("shortDescription");
						try
						{
						longdesc=c1.getString("longDescription");
						}
						catch(Exception e)
						{
							longdesc="Not Available";
						}
						HashMap<String, String> map=new HashMap<String, String>();
						map.put("id", sid);
						map.put("Name", sname);
						map.put("brand", mname);
						map.put("shortdesc", shortdesc);
						map.put("longdesc", longdesc);
						
						trendinglist.add(map);
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
			 
			 itemadapter=new Topitemadapter(FeaturedActivity.this, trendinglist);
			 lvtrending.setAdapter(itemadapter);
			 
			 storeadapter=new Topstoreadapter(FeaturedActivity.this, storelist);
			 lvpopularstore.setAdapter(storeadapter);
			 
			 catadapter=new TopCategoryadapter(FeaturedActivity.this, topcatlist);
			 lvmostcat.setAdapter(catadapter);
		}

	}

	public void gettopstore() 
	{
		ServiceHandler sh = new ServiceHandler();
		String jsonStr = sh.makeServiceCall(apputil.topstoredurl, ServiceHandler.GET);
		//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
		
		Log.d("state Response: ", "> " + jsonStr);

		if (jsonStr != null) {
			try {
				JSONArray contacts = new JSONArray(jsonStr);

				// looping through All Contacts
				for (int i = 0; i <contacts.length() ; i++) {
					JSONObject c = contacts.getJSONObject(i);
					JSONObject c1=c.getJSONObject("objclass");
					String sid = c1.getString("id");
					String sname = c1.getString("Name");
					String stime=c1.getString("createTime");
					
					HashMap<String, String> map=new HashMap<String, String>();
					map.put("id", sid);
					map.put("Name", sname);
					map.put("Time", stime);
					
					storelist.add(map);
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		} else {
			Log.e("ServiceHandler", "Couldn't get any data from the url");
		}
	}
	
	public void gettopcategory() 
	{
		String sid,sname,sctime,slupdate,sview,ssubcatid;
		ServiceHandler sh = new ServiceHandler();
		String jsonStr = sh.makeServiceCall(apputil.topcaturl, ServiceHandler.GET);
		//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
		
		Log.d(" sub category Response: ", "> " + jsonStr);

		if (jsonStr != null) {
			try {
				JSONArray contacts = new JSONArray(jsonStr);

				// looping through All Contacts
				for (int i = 0; i <contacts.length() ; i++) {
					JSONObject c = contacts.getJSONObject(i);
					JSONObject c1=c.getJSONObject("objclass");
					sid = c1.getString("id");
					sname = c1.getString("Name");
					sctime = c1.getString("createTime");
					slupdate = c1.getString("lastUpdated");
					sview = c1.getString("Views");
					ssubcatid = c1.getString("SubCategoryID");
					
					HashMap<String, String> map=new HashMap<String, String>();
					map.put("id", sid);
					map.put("Name", sname);
					map.put("createTime", sctime);
					map.put("lastUpdated", slupdate);
					map.put("Views", sview);
					map.put("SubCategoryID", ssubcatid);
					
					topcatlist.add(map);
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		} else {
			Log.e("ServiceHandler", "Couldn't get any data from the url");
		}
	}
	
	public void showAlertDialog(Context context, String title, String message, Boolean status) {
        AlertDialog alertDialog = new AlertDialog.Builder(context).create();
 
        // Setting Dialog Title
        alertDialog.setTitle(title);
 
        // Setting Dialog Message
        alertDialog.setMessage(message);
         
        // Setting alert dialog icon
        //alertDialog.setIcon((status) ? R.drawable.success : R.drawable.fail);
 
        // Setting OK Button
        alertDialog.setButton("OK", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int which) {
            	
            	dialog.cancel();
            	finish();
            }
        });
 
        // Showing Alert Message
        alertDialog.show();
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
