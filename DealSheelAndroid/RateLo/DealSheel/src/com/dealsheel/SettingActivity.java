package com.dealsheel;

import java.util.ArrayList;
import java.util.HashMap;

import com.utility.AppUtility;
import com.utility.CategoryHelper;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

public class SettingActivity extends Activity{
	
	LinearLayout lnvmain,lnvbody;
	TextView tvtitle,tvchangeloctitle;
	ImageView imgsearch,imgback;
	AppUtility apputil;
	AutoCompleteTextView edsearch;
	ArrayList<HashMap<String, String>> statelist;
	CategoryHelper helper;
	String[] statesname;
	Boolean flag=true;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.setting);
		
		lnvbody=(LinearLayout)findViewById(R.id.lnv_changelocationbody);
		lnvmain=(LinearLayout)findViewById(R.id.lnv_changelocation);
		tvchangeloctitle=(TextView)findViewById(R.id.tv_changelocation_title);
		tvtitle=(TextView)findViewById(R.id.tv_setting_title);
		imgsearch=(ImageView)findViewById(R.id.img_changelocation_search);
		imgback=(ImageView)findViewById(R.id.img_setting_back);
		edsearch=(AutoCompleteTextView)findViewById(R.id.ed_changelocation);
		statelist=new ArrayList<HashMap<String,String>>();
		
		apputil=new AppUtility(getApplicationContext());
		helper=new CategoryHelper(getApplicationContext());
		
		statelist=helper.getstateList();
		statesname=helper.getstatesname();
		Log.e("statelist", statesname+"");
		ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,android.R.layout.simple_dropdown_item_1line, statesname);
        edsearch.setAdapter(adapter);
        
        edsearch.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				// TODO Auto-generated method stub
				
				
				for(int i=0;i<statelist.size();i++)
				{
					String tnm=statelist.get(position).get("name");
					if(tnm.equalsIgnoreCase(statesname[position]))
					{
						if(apputil.setstate(tnm, statelist.get(position).get("id")))
						{
							Toast.makeText(getApplicationContext(),"Location Change to "+ statesname[position], 200).show();
							Log.e("stateid", statelist.get(position).get("id"));
							edsearch.setText("");
							lnvbody.setVisibility(View.GONE);
							lnvmain.setVisibility(View.VISIBLE);
							tvtitle.setText("Settings");
							flag=false;
						}
					}
				}
			}
		});
        
		lnvmain.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				lnvbody.setVisibility(View.VISIBLE);
				lnvmain.setVisibility(View.GONE);
				flag=false;
				tvtitle.setText("Change Location");
				tvchangeloctitle.setText("Current Location :"+apputil.getstatename());
			}
		});
		
		imgback.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if(flag)
				{
				finish();
				//flag=false;
				}
				else
				{
					lnvbody.setVisibility(View.GONE);
					lnvmain.setVisibility(View.VISIBLE);
					tvtitle.setText("Settings");
					flag=true;
				}
			}
		});
		
	}

}
