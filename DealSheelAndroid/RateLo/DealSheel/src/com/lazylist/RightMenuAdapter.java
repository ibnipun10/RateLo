package com.lazylist;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseExpandableListAdapter;
import android.widget.CheckBox;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.SeekBar;
import android.widget.SeekBar.OnSeekBarChangeListener;
import android.widget.TextView;

import com.dealsheel.R;
import com.utility.AppUtility;

public class RightMenuAdapter extends BaseExpandableListAdapter {

	private Activity mActivity;
	ArrayList<HashMap<String, String>> data;
	private static LayoutInflater inflater = null;
	public ImageLoader imageLoader;
	AppUtility apputil;
	static Boolean[][] ischecked;
	static HashMap<String, List<String>> filters;

	public RightMenuAdapter(Activity activity,
			ArrayList<HashMap<String, String>> d) {
		this.mActivity = activity;
		data = d;
		inflater = LayoutInflater.from(mActivity);
		imageLoader = new ImageLoader(mActivity.getApplicationContext());
		apputil = new AppUtility(mActivity.getApplicationContext());
		ischecked = new Boolean[data.size()][100];
		filters = new HashMap<String, List<String>>();
	}

	@Override
	public Object getChild(int groupPosition, int childPosition) {
		// TODO Auto-generated method stub
		return data.get(groupPosition);
	}

	@Override
	public long getChildId(int groupPosition, int childPosition) {
		// TODO Auto-generated method stub
		return childPosition;
	}

	@Override
	public int getChildType(int groupPosition, int childPosition) {
		// TODO Auto-generated method stub
		return Integer.parseInt(data.get(groupPosition).get("isint"));
	}

	@Override
	public View getChildView(final int groupPosition, final int childPosition,
			boolean isLastChild, View convertView, ViewGroup parent) {
		// TODO Auto-generated method stub

		switch (getChildType(groupPosition, childPosition)) {
		case 1: {
			final ViewOneHolder viewOneHolder;

			String value = data.get(groupPosition).get(
					String.valueOf(childPosition));
			final String minValue = value.substring(value.indexOf("[") + 1,
					value.indexOf(","));
			String maxValue = value.substring(value.indexOf(",") + 1,
					value.indexOf("]"));

			if (convertView == null) {
				convertView = inflater.inflate(R.layout.right_menu_seek_child,
						null);
				viewOneHolder = new ViewOneHolder();
				viewOneHolder.seekValue = (SeekBar) convertView
						.findViewById(R.id.sb_filter);
				viewOneHolder.minValue = (TextView) convertView
						.findViewById(R.id.tv_min_val);
				viewOneHolder.maxValue = (TextView) convertView
						.findViewById(R.id.tv_max_val);
				viewOneHolder.currentValue = (TextView) convertView
						.findViewById(R.id.tv_curr_val);
				viewOneHolder.seekValue.setProgress(Integer.parseInt(minValue));
				convertView.setTag(viewOneHolder);
			} else {

				viewOneHolder = (ViewOneHolder) convertView.getTag();
				viewOneHolder.seekValue.setProgress(Integer
						.parseInt(viewOneHolder.currentValue.getText()
								.toString()));
			}
			viewOneHolder.minValue.setText(minValue);
			viewOneHolder.maxValue.setText(maxValue);
			viewOneHolder.currentValue.setText(String
					.valueOf(viewOneHolder.seekValue.getProgress()));
			viewOneHolder.seekValue.setMax(Integer.parseInt(maxValue));
			viewOneHolder.seekValue
					.setOnSeekBarChangeListener(new OnSeekBarChangeListener() {

						@Override
						public void onStopTrackingTouch(SeekBar seekBar) {
							// TODO Auto-generated method stub
							viewOneHolder.currentValue.setText(String
									.valueOf(seekBar.getProgress()));
							ArrayList<String> val = new ArrayList<String>();
							val.add(String.valueOf(seekBar.getProgress()));
							if (!filters.containsKey(data.get(groupPosition)
									.get("Name")))
								filters.put(
										data.get(groupPosition).get("Name"),
										val);
							filters.put(data.get(groupPosition).get("Name"),
									val);
						}

						@Override
						public void onStartTrackingTouch(SeekBar seekBar) {
							// TODO Auto-generated method stub

						}

						@Override
						public void onProgressChanged(SeekBar seekBar,
								int progress, boolean fromUser) {
							if (progress < Integer.parseInt(minValue)) {
								seekBar.setProgress(Integer.parseInt(minValue));
							}

						}
					});
			return convertView;
		}

		case 0: {

			final ViewZeroHolder holder;
			if (convertView == null) {
				convertView = inflater.inflate(R.layout.rightmenu_child_item,
						null);
				holder = new ViewZeroHolder();
				holder.filterText = (TextView) convertView
						.findViewById(R.id.tv_rightmenu_item);
				holder.filterBox = (CheckBox) convertView
						.findViewById(R.id.cb_sub_filter);
				convertView.setTag(holder);
			} else {

				holder = (ViewZeroHolder) convertView.getTag();
			}

			try {

				holder.filterText.setText(data.get(groupPosition).get(
						String.valueOf(childPosition)));
				Boolean checkState = ischecked[groupPosition][childPosition];
				if (checkState != null) {
					holder.filterBox.setChecked(checkState);
				} else {
					holder.filterBox.setChecked(false);
				}
				holder.filterBox.setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View v) {
						// TODO Auto-generated method stub
						ischecked[groupPosition][childPosition] = ((CheckBox) v)
								.isChecked();
						List<String> filterValues = filters.get(data.get(
								groupPosition).get("Name"));
						if (((CheckBox) v).isChecked()) {
							filterValues.add(data.get(groupPosition).get(
									String.valueOf(childPosition)));
						} else {
							filterValues.remove(data.get(groupPosition).get(
									String.valueOf(childPosition)));
						}
					}
				});

			} catch (Exception e) {
				e.printStackTrace();
			}

			return convertView;

		}

		default:
			return convertView;
		}

	}

	@Override
	public int getChildrenCount(int groupPosition) {
		// TODO Auto-generated method stub
		int size = Integer.parseInt(data.get(groupPosition).get("subsize"));
		return size;
	}

	@Override
	public Object getGroup(int groupPosition) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public int getGroupCount() {
		// TODO Auto-generated method stub
		return data.size();
	}

	@Override
	public long getGroupId(int groupPosition) {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public View getGroupView(int groupPosition, boolean isExpanded,
			View convertView, ViewGroup parent) {
		// TODO Auto-generated method stub
		ViewZeroHolder holder;
		String filterName;

		if (convertView == null) {
			convertView = inflater.inflate(R.layout.rightmenu_item, null);
			holder = new ViewZeroHolder();
			holder.filterText = (TextView) convertView
					.findViewById(R.id.tv_rightmenu_item);
			convertView.setTag(holder);
		} else {

			holder = (ViewZeroHolder) convertView.getTag();
		}

		try {

			filterName = data.get(groupPosition).get("Name");
			holder.filterText.setText(filterName);
			if (!filters.containsKey(data.get(groupPosition).get("Name")))
				filters.put(filterName, new ArrayList<String>());

		} catch (Exception e) {
			e.printStackTrace();
		}
		return convertView;
	}

	@Override
	public boolean hasStableIds() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean isChildSelectable(int groupPosition, int childPosition) {
		// TODO Auto-generated method stub
		return true;
	}

	@Override
	public int getChildTypeCount() {
		// TODO Auto-generated method stub
		return 2;
	}

	static class ViewZeroHolder {

		TextView filterText;
		CheckBox filterBox;
		static boolean isChecked = false;

	}

	static class ViewOneHolder {
		SeekBar seekValue;
		TextView minValue;
		TextView maxValue;
		TextView currentValue;
	}

	public HashMap<String, List<String>> getFilterValues() {
		return filters;
	}

}
