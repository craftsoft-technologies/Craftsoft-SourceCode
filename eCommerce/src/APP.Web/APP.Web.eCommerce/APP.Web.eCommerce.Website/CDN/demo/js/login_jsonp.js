function chklogin(result) {
	var vLogout1 = '<a title="会員情報" href="https://www.uniqlo.com/id/member/" class="track_click ga_cat-L1_top ga_act-L1_H_mypage"><img class="imgover" alt="会員情報" src="https://im.uniqlo.com/images/id/pc/img/material/nav/iduq/btn_nav_utility03_login.gif" /></a>';
	var vLogout2 = '<a title="ログアウト" href="https://www.uniqlo.com/hk/store/FSC01010E03.do" class="track_click ga_cat-L1_top ga_act-L1_H_logout"><img class="imgover" alt="ログアウト" src="https://im.uniqlo.com/images/id/pc/img/material/nav/iduq/btn_nav_utility04_login.gif" /></a>';
	var vLogin1 = '<a title="会員登録" href="https://www.uniqlo.com/id/member/registry" class="track_click ga_cat-L1_top ga_act-L1_H_member"><img class="imgover" alt="会員登録" src="https://im.uniqlo.com/images/id/pc/img/material/nav/iduq/btn_nav_utility03.gif" /></a>';
	var vLogin2 = '<a title="ログイン" href="https://www.uniqlo.com/id/store/FSC01010E02.do" class="track_click ga_cat-L1_top ga_act-L1_H_login"><img class="imgover" alt="ログイン" src="https://im.uniqlo.com/images/id/pc/img/material/nav/iduq/btn_nav_utility04.gif" /></a>';

	if (result.loginsts == "T") {
		$("#areaStoreLogin1 > a").remove();
		$(vLogout1).appendTo("#areaStoreLogin1");

		$("#areaStoreLogin2 > a").remove();
		$(vLogout2).appendTo("#areaStoreLogin2");

	} else {
		$("#areaStoreLogin1 > a").remove();
		$(vLogin1).appendTo("#areaStoreLogin1");

		$("#areaStoreLogin2 > a").remove();
		$(vLogin2).appendTo("#areaStoreLogin2");
	}

}