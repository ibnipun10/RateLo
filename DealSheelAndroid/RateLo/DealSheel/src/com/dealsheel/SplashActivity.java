package com.dealsheel;

import java.util.ArrayList;
import java.util.HashMap;

import org.apache.http.NameValuePair;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.utility.AppUtility;
import com.utility.CategoryHelper;
import com.utility.JsonParser;
import com.utility.ServiceHandler;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.View;
import android.widget.LinearLayout;

public class SplashActivity extends Activity {

	Handler handel=new Handler();
	LinearLayout lnv;
	CategoryHelper databaseHelper;
	private ProgressDialog pDialog;
	JSONArray memeberListjsonArray;
	JSONObject RegistrationResultJSONObject = null;
	AppUtility apputil;
	ArrayList<HashMap<String, String>> catlist,statelist,subcatlist,suballcatlist;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		apputil=new AppUtility(getApplicationContext());
		
		setContentView(R.layout.splash);
		
		catlist=new ArrayList<HashMap<String,String>>();
		statelist=new ArrayList<HashMap<String,String>>();
		suballcatlist=new ArrayList<HashMap<String,String>>();
		subcatlist=new ArrayList<HashMap<String,String>>();
		
		
		lnv=(LinearLayout)findViewById(R.id.lnv_splash);
		lnv.setVisibility(View.GONE);
		
		databaseHelper =new CategoryHelper(SplashActivity.this);
		
		
		handel.postDelayed(new Runnable() {
			
			@Override
			public void run() {
				if(apputil.getstatename() != "")
				{
					 startActivity(new Intent(getApplicationContext(), FeaturedActivity.class));
					 finish();
				}
				else
				{
					databaseHelper.deleteAll();
				lnv.setVisibility(View.VISIBLE);
				new GetContacts().execute();
				}
			}
		}, 3000);
		
		
	}

	private class GetContacts extends AsyncTask<Void, Void, Void> {

		String sid,sname,sctime,slupdate,slat,slong;
		@Override
		protected void onPreExecute() {
			super.onPreExecute();
			// Showing progress dialog
			/*pDialog = new ProgressDialog(SplashActivity.this);
			pDialog.setMessage("Please wait...");
			pDialog.setCancelable(false);
			pDialog.show();*/

		}

		@Override
		protected Void doInBackground(Void... arg0) {
			// Creating service handler class instance
			
			getcategory();
			getsubcategory();
			ServiceHandler sh = new ServiceHandler();
			String jsonStr = sh.makeServiceCall(apputil.stateurl, ServiceHandler.GET);
			//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
			
			Log.e("state Response: ", "> " + jsonStr);

			if (jsonStr != null) {
				try {
					JSONArray contacts = new JSONArray(jsonStr);

					// looping through All Contacts
					for (int i = 0; i <contacts.length() ; i++) {
						JSONObject c = contacts.getJSONObject(i);
						
						sid = c.getString("id");
						sname = c.getString("Name");
						sctime = c.getString("createTime");
						slupdate = c.getString("lastUpdated");
						slat = c.getString("Lattitude");
						slong = c.getString("Longitude");
						
						databaseHelper.saveState(sid, sname, sctime, slupdate, slat, slong);
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
						
			
			/* if (pDialog.isShowing())
					pDialog.dismiss();*/
			 
			 startActivity(new Intent(getApplicationContext(), StateActivity.class));
			 finish();
		}

	}

	public void getcategory() 
	{
		String sid,sname,sctime,slupdate;
		ServiceHandler sh = new ServiceHandler();
		String jsonStr = sh.makeServiceCall(apputil.maincaturl, ServiceHandler.GET);
		//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
		
		Log.e(" category Response: ", "> " + jsonStr);

		if (jsonStr != null) {
			try {
				JSONArray contacts = new JSONArray(jsonStr);

				// looping through All Contacts
				for (int i = 0; i <contacts.length() ; i++) {
					JSONObject c = contacts.getJSONObject(i);
					
					sid = c.getString("id");
					sname = c.getString("Name");
					sctime = c.getString("createTime");
					slupdate = c.getString("lastUpdated");
					getsubcategorybucat(sid); 
					databaseHelper.saveCategory(sid, sname, sctime, slupdate);
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		} else {
			Log.e("ServiceHandler", "Couldn't get any data from the url");
		}
	}
	
	private void getsubcategorybucat(String rid) 
	{
		String sid,sname,sctime,slupdate,sview,ssubcatid;
		ServiceHandler sh = new ServiceHandler();
		String jsonStr = sh.makeServiceCall(apputil.subcaturl+rid, ServiceHandler.GET);
		//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
		
		Log.e(" sub category Response: ", "> " + jsonStr);

		if (jsonStr != null) {
			try {
				JSONArray contacts = new JSONArray(jsonStr);

				// looping through All Contacts
				for (int i = 0; i <contacts.length() ; i++) {
					JSONObject c = contacts.getJSONObject(i);
					
					sid = c.getString("id");
					sname = c.getString("Name");
					sctime = c.getString("createTime");
					slupdate = c.getString("lastUpdated");
					sview = c.getString("Views");
					ssubcatid = c.getString("SubCategoryID");
					
					databaseHelper.savesubcat(sid, sname, sctime, slupdate, sview, ssubcatid,rid);
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		} else {
			Log.e("ServiceHandler", "Couldn't get any data from the url");
		}
	}

	public void getsubcategory() 
	{
		String sid,sname,sctime,slupdate,sview,ssubcatid;
		ServiceHandler sh = new ServiceHandler();
		String jsonStr = sh.makeServiceCall(apputil.subcatAllurl, ServiceHandler.GET);
		//String jsonStr = sh.makeServiceCall(AppUtility.productlisturl, ServiceHandler.GET);
		
		Log.e(" sub category Response: ", "> " + jsonStr);

		if (jsonStr != null) {
			try {
				JSONArray contacts = new JSONArray(jsonStr);

				// looping through All Contacts
				for (int i = 0; i <contacts.length() ; i++) {
					JSONObject c = contacts.getJSONObject(i);
					
					sid = c.getString("id");
					sname = c.getString("Name");
					sctime = c.getString("createTime");
					slupdate = c.getString("lastUpdated");
					sview = c.getString("Views");
					ssubcatid = c.getString("SubCategoryID");
					
					databaseHelper.saveAllsubcat(sid, sname, sctime, slupdate, sview, ssubcatid);
				}
			} catch (JSONException e) {
				e.printStackTrace();
			}
		} else {
			Log.e("ServiceHandler", "Couldn't get any data from the url");
		}
	}
}
