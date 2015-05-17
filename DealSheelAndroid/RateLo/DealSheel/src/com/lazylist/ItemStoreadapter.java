package com.lazylist;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Locale;

import com.dealsheel.R;
import com.dealsheel.StoreMapActivity;
//import com.google.android.gms.internal.lv;
import com.utility.AppUtility;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.BaseExpandableListAdapter;
import android.widget.ExpandableListAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.SimpleExpandableListAdapter;
import android.widget.TextView;

public class ItemStoreadapter extends BaseExpandableListAdapter {

	private Activity activity;
	ArrayList<HashMap<String, String>> data;
	private static LayoutInflater inflater = null;
	public ImageLoader imageLoader;
	AppUtility apputil;
	Collection<String> storeGroups;
	ArrayList<ArrayList<HashMap<String, String>>> childItems;
	public ItemStoreadapter(Activity a, ArrayList<HashMap<String, String>> d) {
		activity = a;
		data = d;
		inflater = (LayoutInflater) activity
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		imageLoader = new ImageLoader(activity.getApplicationContext());
		apputil = new AppUtility(activity.getApplicationContext());
		storeGroups = new HashSet<String>();
		storeGroups = new HashSet<String>();
		for (int i = 0; i < data.size(); i++) {
			storeGroups.add(data.get(i).get("storeid"));
		}		
		childItems= new ArrayList<ArrayList<HashMap<String,String>>>(storeGroups.size());
	}

	@Override
	public void notifyDataSetChanged() {
		// TODO Auto-generated method stub
		super.notifyDataSetChanged();
		Collection<String> storeGroups = new HashSet<String>();
		for (int i = 0; i < data.size(); i++) {
			storeGroups.add(data.get(i).get("storeid"));
		}
	}

	public View getView(final int position, View convertView, ViewGroup parent) {		
		return convertView;
	}

	private String getDate(long timeStamp) {

		SimpleDateFormat sdf = new SimpleDateFormat("dd-MMM-yy", Locale.US);
		Date netDate = (new Date(timeStamp));
		Log.e("Date is", sdf.format(netDate));
		return sdf.format(netDate);

	}

	@Override
	public int getGroupCount() {
		// TODO Auto-generated method stub
		return storeGroups.size();
	}

	@Override
	public int getChildrenCount(int groupPosition) {
		// TODO Auto-generated method stub
		int size = 0;
		String key=storeGroups
				.toArray(new String[storeGroups.size()])[groupPosition];
		ArrayList<HashMap<String, String>> groupChilds= new ArrayList<HashMap<String,String>>();
		for (int i = 0; i < data.size(); i++) {			
			if (data.get(i).get("storeid").equals(key)) {				
				size++;
				groupChilds.add(data.get(i));
			}
		}		
		childItems.add(groupPosition, groupChilds);
		return size;
	}

	@Override
	public Object getGroup(int groupPosition) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Object getChild(int groupPosition, int childPosition) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public long getGroupId(int groupPosition) {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public long getChildId(int groupPosition, int childPosition) {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public boolean hasStableIds() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public View getGroupView(int groupPosition, boolean isExpanded,
			View convertView, ViewGroup parent) {
		// TODO Auto-generated method stub
		GroupViewHolder groupViewHolder;
		if (convertView == null) {
			convertView = LayoutInflater.from(activity).inflate(
					R.layout.row_store_group, null);
			groupViewHolder = new GroupViewHolder();
			groupViewHolder.storeImage = (ImageView) convertView
					.findViewById(R.id.iv_storegroup);
			groupViewHolder.storeName = (TextView) convertView
					.findViewById(R.id.tv_store_group_name);
			groupViewHolder.storeBranchCount = (TextView) convertView
					.findViewById(R.id.tv_store_group_branch_count);
			convertView.setTag(groupViewHolder);
		} else {
			groupViewHolder = (GroupViewHolder) convertView.getTag();
		}
		

		String[] stores = storeGroups.toArray(new String[storeGroups.size()]);
		String key = stores[groupPosition];
		for (int i = 0; i < data.size(); i++) {
			if (data.get(i).get("storeid") == key) {
				String imageURL = data.get(i).get("storeid");
				if (imageURL != null && !imageURL.equals("")) {
					try {
						imageLoader.DisplayImage(apputil.topstoreimagesurl
								+ imageURL + ".jpg", groupViewHolder.storeImage);
					} catch (Exception e) {
						e.printStackTrace();
					}
				}				
				groupViewHolder.storeName.setText(data.get(i).get("name"));
				groupViewHolder.storeBranchCount.setText("Available in "+getChildrenCount(groupPosition)+" Store"+(getChildrenCount(groupPosition)>1?"s":""));
				
			}
		}

		return convertView;
	}

	@Override
	public View getChildView(final int groupPosition, final int childPosition,
			boolean isLastChild, View convertView, ViewGroup parent) {
		// TODO Auto-generated method stub
		if (convertView == null)
			convertView = inflater.inflate(R.layout.storelist_item, null);

		ImageView imageview = (ImageView) convertView
				.findViewById(R.id.img_storelistitem_image);
		TextView tv_name = (TextView) convertView
				.findViewById(R.id.tv_storelistitem_name);
		TextView tv_address = (TextView) convertView
				.findViewById(R.id.tv_storelistitem_address);
		TextView tv_phone = (TextView) convertView
				.findViewById(R.id.tv_storelistitem_phoneno);
		TextView tv_desc = (TextView) convertView
				.findViewById(R.id.tv_storelistitem_detail);
		TextView tv_price = (TextView) convertView
				.findViewById(R.id.tv_storelistitem_price);
		TextView tv_date = (TextView) convertView
				.findViewById(R.id.tv_storelistitem_date);
		TextView tv_distance = (TextView) convertView
				.findViewById(R.id.tv_storelistitem_distance);
		LinearLayout lnvcall = (LinearLayout) convertView
				.findViewById(R.id.lnv_call);
		LinearLayout lShowMap = (LinearLayout) convertView
				.findViewById(R.id.ll_showmap);

		try {

			
			String tempurl = childItems.get(groupPosition).get(childPosition).get("storeid");
			if (tempurl != null && !tempurl.equals("")) {
				try {
					imageLoader.DisplayImage(apputil.topstoreimagesurl
							+ tempurl + ".jpg", imageview);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
			
			tv_name.setText(childItems.get(groupPosition).get(childPosition).get("name"));
			tv_address.setText(childItems.get(groupPosition).get(childPosition).get("address"));
			tv_phone.setText(childItems.get(groupPosition).get(childPosition).get("phone"));
			tv_desc.setText(childItems.get(groupPosition).get(childPosition).get("desc"));
			tv_price.setText("\u20B9 " + childItems.get(groupPosition).get(childPosition).get("price"));

			long timestamp = Long.parseLong(childItems.get(groupPosition).get(childPosition).get("itemtime")) * 1000;
			tv_date.setText("As of " + getDate(timestamp));
			tv_distance.setText(childItems.get(groupPosition).get(childPosition).get("distance") + "KM");

			lnvcall.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					// TODO Auto-generated method stub

					String intValue = childItems.get(groupPosition).get(childPosition).get("phone")
							.replaceAll("[a-zA-Z]", "");
					if (!intValue.trim().equalsIgnoreCase("")
							&& intValue.trim() != null) {
						Log.e("callnumber", intValue.trim());
						try {
							Intent callIntent = new Intent(Intent.ACTION_CALL);
							callIntent.setData(Uri.parse("tel:"
									+ intValue.trim()));
							activity.startActivity(callIntent);
						} catch (Exception e) {
							e.printStackTrace();
						}
					}
				}
			});

			convertView.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					// TODO Auto-generated method stub
					String tempurl = childItems.get(groupPosition).get(childPosition).get("storeid");
					Intent i = new Intent(activity.getApplicationContext(),
							StoreMapActivity.class);
					i.putExtra("storeid", childItems.get(groupPosition).get(childPosition).get("storeid"));
					i.putExtra("storename", childItems.get(groupPosition).get(childPosition).get("name"));
					i.putExtra("storeimage", apputil.topstoreimagesurl
							+ tempurl + ".jpg");
					Log.e("image send", apputil.topstoreimagesurl + tempurl
							+ ".jpg");
					i.putExtra("storelat", childItems.get(groupPosition).get(childPosition).get("lat"));
					i.putExtra("storelong", childItems.get(groupPosition).get(childPosition).get("long"));
					i.putExtra("storephone", childItems.get(groupPosition).get(childPosition).get("phone"));
					i.putExtra("storeaddress", childItems.get(groupPosition).get(childPosition)
							.get("address"));
					activity.startActivity(i);
				}
			});

		} catch (Exception e) {
			e.printStackTrace();
		}
		return convertView;
	}

	@Override
	public boolean isChildSelectable(int groupPosition, int childPosition) {
		// TODO Auto-generated method stub
		return false;
	}

	static class GroupViewHolder {
		ImageView storeImage;
		TextView storeName;
		TextView storeBranchCount;
	}

}