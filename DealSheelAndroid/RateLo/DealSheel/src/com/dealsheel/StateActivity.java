package com.dealsheel;

import java.util.ArrayList;
import java.util.HashMap;

import com.utility.AppUtility;
import com.utility.CategoryHelper;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.ListView;

public class StateActivity extends Activity {

	ImageView img;
	CategoryHelper helper;
	ArrayList<HashMap<String, String>> statelist;
	String[] names;
	AppUtility apputil;
			
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.stateselect);
		
		
		img=(ImageView)findViewById(R.id.img_setlocation);
		helper=new CategoryHelper(StateActivity.this);
		statelist=new ArrayList<HashMap<String,String>>();
		apputil=new AppUtility(getApplicationContext());
		statelist=helper.getstateList();
		
		img.setOnClickListener(new OnClickListener() {
			
			@SuppressLint("NewApi")
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				ArrayList<String> imgPaths = new ArrayList<String>();
				for(int i=0;i<statelist.size();i++)
				{					
					imgPaths.add(statelist.get(i).get("name"));
				}
				
		        AlertDialog.Builder alertDialog = new AlertDialog.Builder(StateActivity.this);
		        LayoutInflater inflater = getLayoutInflater();
		        View convertView = (View) inflater.inflate(R.layout.dialoglist, null);
		        alertDialog.setView(convertView);
		        alertDialog.setTitle("Select Your Location");
		        ListView lv = (ListView) convertView.findViewById(R.id.lv_dialog);
		        ArrayAdapter<String> adapter = new ArrayAdapter<String>(StateActivity.this,R.layout.row_city_list,imgPaths);
		        lv.setAdapter(adapter);
		        alertDialog.show();
		        
		        lv.setOnItemClickListener(new OnItemClickListener() {

					@Override
					public void onItemClick(AdapterView<?> parent, View view,
							int position, long id) {
						// TODO Auto-generated method stub
						
						if(apputil.setstate(statelist.get(position).get("name"),statelist.get(position).get("id")))
						{
							startActivity(new Intent(StateActivity.this, FeaturedActivity.class));
							finish();
						}
					}
				});
			}
		});
	}

}
