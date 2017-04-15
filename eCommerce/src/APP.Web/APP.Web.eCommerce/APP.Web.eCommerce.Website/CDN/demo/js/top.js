/**
 * UNIQLO L1 Scripts
 * 
 * @author  katsuma@team-lab.com
 * @version 1.2.2
 * @require jquery 1.6
 * @require jquery easing 1.3
 */

/*
 * uniqlo utils
 */
if(location.href.indexOf("file:///")==0 && window.console){
	$.extend({log: function(msg){console.log(msg)}});
}else{
	$.extend({log: function(){}});
}

/*
 * image rollover plugin
 */
(function($){
	$.fn["rollover"] = function(options){
		var settings = $.extend({
			"suffix" : "_o"
		}, options);
		
		var _imgs = [];
		
		this.each(function(){
			var dsrc  = $(this).attr('src');
			var ftype = dsrc.substring(dsrc.lastIndexOf('.'), dsrc.length);
			var hsrc  = (dsrc.indexOf(settings.suffix+ftype)>0) ? dsrc:dsrc.replace(ftype, settings.suffix+ftype);
			// preload
			_imgs.push(new Image());
			_imgs[_imgs.length-1].src = hsrc;
			
			$(this)
				.attr({"dsrc": dsrc, "hsrc": hsrc})
				.mouseover(function(){$(this).attr("src", $(this).attr("hsrc"));})
				.mouseout( function(){$(this).attr("src", $(this).attr("dsrc"));});
		});
		return this;
	};
})(jQuery);

/*
 * image rollover toggle plugin
 * @require image rollover plugin
 */
(function($){
	$.fn["rolloverToggle"] = function(options){
		var settings = $.extend({
			"current": true,
			"suffix" : "_o"
		}, options);
		
		this.each(function(){
			if(settings.current){
				// default -> hover
				$(this).attr({
					"dsrc": $(this).attr("hsrc"),
					"src" : $(this).attr("hsrc")
				});
			}else{
				// hover -> default
				$(this).attr({
					"dsrc": $(this).attr("hsrc").replace(settings.suffix,""),
					"src" : $(this).attr("hsrc").replace(settings.suffix,"")
				});
			}
		});
		return this;
	};
})(jQuery);

/*
 * image loader with callback plugin
 */
(function($){
	$.fn["imageLoader"] = function(callback){
		var _length = $(this).length;
		var _loaded = function(){
			_length--;
			if(_length==0) callback.apply();
		};
		this.each(function(){
			$("<img>")
				.attr("src", $(this).attr("src"))
				.load(_loaded)
				.error(_loaded);
		});
		return this;
	};
})(jQuery);

/*
 * L1 uniqlo header plugin
 * @require image rollover plugin
 */
(function($){
	$.fn["l1header"] = function(options){
		// settings
		var settings = $.extend({
			"imgClass" : ".imgover",
			"mainID"   : "#navHeader",
			"subClass" : "navCategory",
			"cartID"   : "#gnav_cart_trigger",
			"searchID" : "#searchFocus",
			"duration" : 200,
			"animate"  : false
		}, options);

		// main
		this.each(function(){
			// ヘッダ第二階層で遷移しないのはクリックころす
			$(this).find("a").focus(function(){
				$(this).blur();
			});
			$(this).find("ul.navCategory>li>a").click(function(){
				if($(this).attr("href")=="#") return false;
			});
			
			// 検索フォーム
			$(this)
				.find(settings.searchID)
				.focus(function(){
					$(this).parent().parent().addClass("inputFocus");
				})
				.blur(function(){
					$(this).parent().parent().removeClass("inputFocus");
				});
			
			// 第二階層の開閉
			var _timer_header_toggle;
			$("#navHeader").hover(
				function(){
					clearTimeout(_timer_header_toggle);
					$("#header").stop().animate({height:100}, 400, "easeOutQuint", function(){
						$("#header").css({"overflow":"visible"});
					});
				},
				function(){
					_timer_header_toggle = setTimeout(function(){
						$("#header").stop().delay(100).animate({height:50}, 400, "easeOutQuint", function(){
							$("#header").css({"overflow":"hidden"});
						});
					}, 500);
				});
			
			// 第二階層の遅延クローズ用カレント表示保持
			$("#navHeader>li").mouseover(function(){
				$("#navHeader>li").removeClass("current");
				$(this).addClass("current");
			});
			
			// ３階層ドロップダウン
			$(settings.mainID).find("li").hover(
				function(){
					if(!settings.animate && $(this).parent().attr("id") == settings.mainID.replace("#","")){
						$(this).find(">ul").show();
					}else{
						$(this).find(">ul:not(:animated)").slideDown(settings.duration);
					}
				},
				function(){
					// カレントカテゴリ第二階層は閉じない
					if($(this).find(">ul").hasClass(settings.subClass)) return;
					if(!settings.animate && $(this).parent().attr("id") == settings.mainID.replace("#","")){
						$(this)
							.find(".sub")
							.hide()
							.end()
							.find(">ul")
							.hide();
					}else{
						$(this).find(">ul").slideUp(settings.duration, function(){
							// アニメーションのタイミング次第で第三階層が取り残されるので強制的に閉じる
							$(this).find(".sub").hide();
						});
					}
				});
			
			// ロールオーバー変更
			$(this).find(settings.imgClass)
				.unbind("mouseover")
				.unbind("mouseout")
				.parent()
				.parent()
				.hover(
					function(){
						var $img = $(this).find(">a>img");
						$img.attr("src", $img.attr("hsrc"));
					},
					function(){
						var $img = $(this).find(">a>img");
						$img.attr("src", $img.attr("dsrc"));
					});
		});
		return this;
	};
})(jQuery);

/*
 * L1 banner with category filter plugin
 * @require image rollover toggle plugin
 * @require image loader with callback plugin
 */
(function($){
	$.fn["l1Banner"] = function(options){
		// settings
		var settings = $.extend({
			"animate"      : false,
			"interval"     : 4000,
			"slideSpeed"   : 750,
			"click"        : false,
			"thumbsToggle" : true,
			"pauseFade"    : true
		}, options);
		
		// サムネイルカレント表示のアニメーション設定（ie7,8とiPad ver7未満、android tabはアニメーションOFF）
		var ua = navigator.userAgent;
		if((!settings.animate && $.support.opacity) && !ua.match(/ipad/i) && !(ua.match(/android/i) && !ua.match(/mobile/i))){
			settings.animate = true;
		}
		
		// china width 950 fix
		var _content_width = $("#content").width();
		var _content_height_min = $("#content_banner_top .content_banner_inner").height();
		var _content_height_max = _content_height_min + $(".content_banner_thumbs").height();
		
		var _content_banner_category;
		var _content_banner_current = 0;
		// カテゴリフィルタの固定リンクハッシュをヘッダのAタグから抽出
		var _content_location_permalinks = [];
		$("#navHeader>li>a").each(function(){
			_content_location_permalinks.push("#"+$(this).attr("href").split("#")[1]);
		});
		
		var _timer_content_banner_loading;
		var _timer_content_banner_rotation;
		var _timer_content_banner_toggle;
		var _is_mouseover_header  = false;
		var _banner_thumb_handler = (settings.click) ? "click" : "mouseover";
		
		// カテゴリ選択初期化
		var _initialize = function(callback){
			// location.hash判定
			var c, l = location.hash.replace("#!","").toLowerCase();
			if($.inArray(location.hash, _content_location_permalinks)!=-1){
				_content_banner_category = "#content_banner_"+l;
				c = ".category_"+l;
				$("#header_"+l)
					.addClass("selected")
					.find(">img")
					.rolloverToggle()
					.end()
					.find("+ul.navCategory")
					.show();
				
				$("#header")
					.css({"overflow":"hidden"})
					.stop()
					.delay(400)
					.animate({height:50}, 400, "easeOutQuint", function(){
						$("#header").css({"overflow":"visible"});
					});
			}else{
				_content_banner_category = "#content_banner_top";
				c = "";
				$("#top").addClass("selected");
				$("#content_banner_loading").hide();
			}
			// カレントバナー切替
			$("#content_banner>div")
				.filter(":not(#content_banner_loading)")
				.hide()
				.end()
				.filter(_content_banner_category)
				.show();
			
			// カレントコンテンツ切替
			if(c!=""){
				if(!settings.animate){
					$("#content_blocks>.content_block_list>li:not("+c+")").hide();
				}else{
					$("#content_blocks>.content_block_list>li:not("+c+")").slideUp(600, "easeOutQuint");
				}
			}
			
			// バナーが無限ループできるよう先頭要素のクローンを末尾に追加
			$("#content_banner>div").each(function(){
				var $imgs = $(this).find(".content_banner_item");
				if($imgs.length<=1) return;
				$imgs.eq(0).clone(true).appendTo($(this).find(".content_banner_inner"));
				$(this).find(".content_banner_inner").css("width", _content_width*($imgs.length+1));
			});
			
			// カレントバナー画像の読み込み完了したらローテーション開始
			if($.support.opacity){
				$(_content_banner_category+" img").imageLoader(function(){
					$("#content_banner_loading").delay(400).fadeOut(400);
					_timer_content_banner_rotation = setTimeout(_rotateBanner, settings.interval);
					callback.apply();
				});
			// IE7,8は画像読み込み待ちしない
			}else{
				$("#content_banner_loading").delay(400).fadeOut(400);
				_timer_content_banner_rotation = setTimeout(_rotateBanner, settings.interval);
				callback.apply();
			}
		};
		
		// 自動ローテーション
		var _rotateBanner = function(){
			_timer_content_banner_rotation = null;
			_content_banner_current++;
			var $target = $(_content_banner_category + " .content_banner_inner");
			var l = $(_content_banner_category+" .content_banner_item").length-1;

			//20141009 ADD swipe対応
			var ua = navigator.userAgent;
			//iPad or Android Tab
			if (ua.match(/ipad/i) || (ua.match(/android/i) && !ua.match(/mobile/i))){
				l--;
			}

			$target.stop().animate({left:-_content_width*_content_banner_current}, settings.slideSpeed, "easeInOutQuart", function(){
				var c = parseInt($(this).css('left').replace('px')) / -_content_width;
				// 末尾までローテーションしたらコールバックで先頭に戻す
				if(c == l) $target.css({'left':0});
				_rotateStart();
			});
			if(_content_banner_current==l) _content_banner_current = 0;
			// サムネイル部のカレント表示を更新
			if(!settings.animate){
				$(_content_banner_category+">.content_banner_thumbs .content_banner_thumb")
					.eq(_content_banner_current)
					.find(".content_banner_thumb_current>img")
					.css({top:0})
					.end()
					.siblings()
					.find(".content_banner_thumb_current>img")
					.css({top:38});
			}else{
				$(_content_banner_category+">.content_banner_thumbs .content_banner_thumb")
					.eq(_content_banner_current)
					.find(".content_banner_thumb_current>img")
					.stop()
					.delay(settings.slideSpeed/2)
					.animate({top:0}, settings.slideSpeed/2, "easeOutQuint")
					.end()
					.siblings()
					.find(".content_banner_thumb_current>img")
					.stop()
					.delay(settings.slideSpeed/2)
					.animate({top:38}, settings.slideSpeed/2, "easeOutQuint");
			}
		};
		
		// バナーカテゴリ切替
		var _switchBanner = function(cat){
			$.log("category current: " + cat);
			_rotateStop();
			
			// サムネイル部のアニメーション切替
			if(settings.thumbsToggle){
				// サムネイルエリアを閉じる
				$("#content_banner").stop().animate({height:_content_height_min}, 400, "easeOutQuart");
			}else{
				// サムネイルエリアをフェードアウト
				$(".content_banner_thumbs").stop().animate({opacity:0}, 300);
			}
			
			// ローディング
			clearTimeout(_timer_content_banner_loading);
			_timer_content_banner_loading = setTimeout(function(){
				$("#content_banner_loading")
					.stop()
					.fadeOut(200, _rotateStart);
				$(".content_banner_thumbs").stop().animate({opacity:1}, 200);
			}, 500);
			
			$("#content_banner_loading")
				.stop()
				.css({opacity:($("#content_banner_loading").css("opacity")==1)?0:$("#content_banner_loading").css("opacity")})
				.show()
				.animate({opacity:1}, 250, function(){
					$(_content_banner_category).hide();
					_content_banner_current = 0;
					_content_banner_category = "#content_banner_" + cat;
					
					// 表示する前に初期化
					$(_content_banner_category+" .content_banner_inner")
						.stop(true, true)
						.css({'left':0})
						.parent()
						.parent()
						.find(">.content_banner_thumbs")
						.find(">.content_banner_thumb:not(.first_thumb) .content_banner_thumb_current>img")
						.stop(true, true)
						.css({top:38})
						.end()
						.find(">.content_banner_thumb.first_thumb .content_banner_thumb_current>img")
						.stop(true, true)
						.css({top:0})
						.closest(_content_banner_category)
						.show();
				});
		};
		
		// ローテーションタイマー開始
		var _rotateStart = function(){
			if(_is_mouseover_header || $(_content_banner_category+" .content_banner_item").length<=1){
				_rotateStop();
				return;
			}
			if(_timer_content_banner_rotation == null){
				$.log("banner: rotate timer start");
				_timer_content_banner_rotation = setTimeout(_rotateBanner, settings.interval);
			}
		};
		
		// ローテーションタイマー停止
		var _rotateStop = function(){
			if(_timer_content_banner_rotation != null){
				$.log("banner: rotate timer stop");
				clearTimeout(_timer_content_banner_rotation);
				_timer_content_banner_rotation = null;
			}
		};
		
		// main
		_initialize(function(){
			
			// ヘッダでのカテゴリフィルタ
			var speed_  = 600;
			var easing_ = "easeOutQuint";
			
			// top
			$("#top")
				.click(function(){
					// カレント判定
					if($(this).hasClass("selected")) return false;
					$("#navHeader>li>a").removeClass("selected");
					$(this).addClass("selected");
					
					// 第二階層を閉じる
					$("#navHeader .sub").hide();
					$("#header").delay(200).stop().delay(100).animate({height:50}, 400, "easeOutQuint", function(){
						$("#header").css({"overflow":"hidden"});
					});
					
					// ヘッダ切替
					$("#navHeader>li>a>img").rolloverToggle({current:false});
					
					// バナー切替
					_switchBanner($(this).attr("id"));
					
					// コンテンツ切替
					if(!settings.animate){
						$("#content_blocks>.content_block_list>li")
							.show();
					}else{
						$("#content_blocks>.content_block_list>li")
							.stop(true, true)
							.slideDown(speed_, easing_);
					}
					return false;
				});
				
			// category
			$("#navHeader>li>a")
				.click(function(){
					// カレント判定
					if($(this).hasClass("selected")) return false;
					$("#top, #navHeader>li>a").removeClass("selected");
					$(this).addClass("selected");
					
					// 第二階層の重なり順制御
					$(this)
						.find("+ul a.selected")
						.removeClass("selected")
						.find(">img")
						.rolloverToggle({current:false})
						.end()
						.end()
						.parent()
						.addClass("current")
						.siblings()
						.removeClass("current");
					// 第二階層を開く
					//$("#navHeader .sub").hide();
					$("#navHeader a.selected+.sub").slideUp(200);
					$(this).parent().find(">ul").show();
					// ヘッダL2.5が閉じていて、クリックした要素が、絶対パスでない場合
					if($("#header").css("height")!="50px" && $(this).attr('href').indexOf('http://') == -1){
						$("#header").delay(200).stop().animate({height:50}, 400, "easeOutQuint", function(){
							$("#header").css({"overflow":"visible"});
						});
					}
					// (IDが設置されている場合)
					if($(this).attr("id")){
					// ヘッダ切替
					$("#navHeader>li>a:not(#"+$(this).attr("id")+")>img").rolloverToggle({current:false});
					$(this).find(">img").rolloverToggle();
					
					// バナー切替
					var cat = $(this).attr("id").replace("header_","");
					_switchBanner(cat);
					
					// コンテンツ切替
					var c = ".category_" + cat;
					if(!settings.animate){
						$("#content_blocks>.content_block_list>li")
							.filter(":not("+c+")")
							.hide()
							.end()
							.filter(c)
							.show();
					}else{
						$("#content_blocks>.content_block_list>li")
							.filter(":not("+c+")")
							.stop(true, true)
							.slideUp(speed_, easing_)
							.end()
							.filter(c)
							.stop(true, true)
							.slideDown(speed_, easing_);
					}
					}
					// 絶対パス記載がない場合は、クリックでの遷移を無効化
					if($(this).attr('href').indexOf('http://') == -1)return false;
				});
			
			// ヘッダとバナーエリアへのマウスオーバーでバナーローテーション停止
			$("#header>.navArea, #content_banner, #content_banner>div .content_banner_thumbs a").hover(
				function(){
					_is_mouseover_header = true;
					_rotateStop();
					if(settings.pauseFade){
						$("#content_blocks .contentCrossFade").crossFade({stop:true});
					}
				},
				function(){
					_is_mouseover_header = false;
					_rotateStart();
					if(settings.pauseFade){
						$("#content_blocks .contentCrossFade").crossFade();
					}
				}
			);
			
			// バナーエリアへのマウスオーバーでサムネイル開閉
			if(settings.thumbsToggle){
				$("#content_banner").hover(
					function(){
						_rotateStop();
						clearTimeout(_timer_content_banner_toggle);
						$(this)
							.stop()
							.animate({height:_content_height_max}, 600, "easeOutQuart");
					},
					function(){
						_rotateStart();
						_timer_content_banner_toggle = setTimeout(function(){
							$("#content_banner")
								.stop()
								.animate({height:_content_height_min}, 400, "easeOutQuart");
						}, 400);
					});
			}
			
			// サムネイルをクリックorマウスオーバーでバナーのカレントを移動
			$("#content_banner>div").each(function(){
				$(this).find(".content_banner_thumbs a")
					.bind(_banner_thumb_handler, function(){
						var h = $(this).find(".content_banner_thumb_current").height();
						if(!settings.animate){
							$(this)
								.find(".content_banner_thumb_current>img")
								.css({top:0})
								.end()
								.parent()
								.siblings()
								.find("a>.content_banner_thumb_current>img")
								.css({top:h});
						}else{
							$(this)
								.find(".content_banner_thumb_current>img")
								.stop()
								.animate({top:0}, 200, "easeOutQuart")
								.end()
								.parent()
								.siblings()
								.find("a>.content_banner_thumb_current>img")
								.stop()
								.animate({top:h}, 250, "easeOutQuart");
						}
					})
					.each(function(i, elem){
						$(this)
							.bind(_banner_thumb_handler, function(){
								$(_content_banner_category+" .content_banner_inner")
									.stop()
									.animate({left:(-_content_width*i)}, settings.slideSpeed, "easeOutQuart");
								_content_banner_current = i;
								$.log("banner current: "+i.toString());
								return false;
							});
					});
			});
			
			// バナー左右の次へ／前へ
			$("#content_banner_nav_prev, #content_banner_nav_next")
				.mouseover(function(){
					$("#content_banner").trigger("mouseover");
				})
				.mouseout(function(){
					$("#content_banner").trigger("mouseout");
				})
				.click(function(){
					var i = _content_banner_current;
					if($(this).attr("id")=="content_banner_nav_prev") i--;
					if($(this).attr("id")=="content_banner_nav_next") i++;
					$(_content_banner_category+">.content_banner_thumbs a:eq(" + i.toString() +")")
						.trigger(_banner_thumb_handler);
				});

				/****************************
				* add 20140717
				* update 20140723
				* update 20141009
				* バナー左右の次へ／前へ (タブレット対応)
				****************************/
				(function() {
					var ua = navigator.userAgent;

					//iPad or Android Tab
					if (ua.match(/ipad/i) || (ua.match(/android/i) && !ua.match(/mobile/i))){
						var targetBlock = $("#content_banner>div>div.content_banner_wrap>ul.content_banner_inner");
//						targetBlock.css("padding-left", "960px").css("margin-left", "-960px");
						var visiableBlock = $("#content_banner>div>div.content_banner_wrap>ul.content_banner_inner:visible");
						var touchEventPoint = 0;

						//init position (set number format)
						var leftPosition = parseInt(targetBlock.css("left"));
						if (!leftPosition) {
							targetBlock.css("left", 0);
							leftPosition = 0;
						}
						
						//bind event
						targetBlock.each(function(){
							var $target = $(this);
							$target.bind("touchstart touchmove touchend", touchHandler);
							var count = $target.find('>li').length;
							if (count > 1) {
								$target.css("margin-left", "-960px").css("width", "+=960px");
							} else {
								$target.css("width", "+=960px");
							}
							var cloneObj = $target.find('>li:eq('+ (count-2) +')').clone(true);
							touchEventPoint = 1;
							$target.prepend(cloneObj);
//							$target.css("background", "url("+$target.find('>li:eq('+(count-2)+') >div > img').attr('src')+") left center" );
						});

						var startX = 0;
						var startY = 0;
						var pageX = 0;
						var pageY = 0;
						var itemLength = 0;

						function touchHandler(e){
							var touch = e;
							if(touch.type == "touchstart"){
								pageX = touch.originalEvent.changedTouches[0].pageX;
								pageY = touch.originalEvent.changedTouches[0].pageY;
								startX = pageX;
								startY = pageY;
								visiableBlock = $("#content_banner>div>div.content_banner_wrap>ul.content_banner_inner:visible");
								leftPosition = parseInt(visiableBlock.css("left"));
								_rotateStop();
							}
							if(touch.type == "touchmove"){
								itemLength = (visiableBlock.find(">li.content_banner_item:visible").length - 1);
								pageX = touch.originalEvent.changedTouches[0].pageX;
								pageY = touch.originalEvent.changedTouches[0].pageY;
								var movePosition = parseInt((startX - pageX)*-1);
								if (Math.abs(startX - pageX) > 10) {
									e.preventDefault();
								}
								if (itemLength) {
									visiableBlock.css("left", leftPosition + movePosition);
								}
							}
							if(touch.type == "touchend"){
								var i = _content_banner_current;
								if (Math.abs(pageX - startX) > 150) {
									if(pageX > startX){
										i--;
									}else{
										i++;
									}
									//loop
									if (itemLength) {
										if ((i+touchEventPoint) == itemLength) {
											visiableBlock.stop().animate({
												"left": ((itemLength-touchEventPoint)*960*-1)+"px"
											}, 400, function(){
												visiableBlock.css("left", 0);
												setTimeout(function(){
												$(_content_banner_category+">.content_banner_thumbs a:eq(0)").trigger(_banner_thumb_handler);
												},50);
											});
										} else if (i < 0) {
											visiableBlock.stop().animate({
												"left": "960px"
											}, 400, function(){
												visiableBlock.css("left", ((itemLength-(1+touchEventPoint))*960*-1)+"px");
												setTimeout(function(){
												$(_content_banner_category+">.content_banner_thumbs a:eq("+ (itemLength-(1+touchEventPoint)) +")").trigger(_banner_thumb_handler);
												},50);
											});
										} else {
											$(_content_banner_category+">.content_banner_thumbs a:eq(" + (i%itemLength).toString() +")").trigger(_banner_thumb_handler);
										}
									} else {
										i = 0;
									}
								} else {
									$(_content_banner_category+">.content_banner_thumbs a:eq(" + (_content_banner_current).toString() +")").trigger(_banner_thumb_handler);
								}
								setTimeout(function(){
									_rotateStart();
								}, 300);
							}
						}

					}
				})();

		});
		return this;
	};
})(jQuery);

/*
 * L1 banner floating menu plugin
 */
(function($){
	$.fn["stalker"] = function(options){
		// settings
		var settings = $.extend({
			"className" : "stalker",
			"duration"  : 1000,
			"easing"    : "easeOutExpo",
			"timer"     : null
		}, options);
		
		// follow mouse cursor
		var _stalking = function(e, $c, $s) {
			var o = $c.offset();
			var x = e.clientX - o.left - ($s.width()/2);
			var y = e.clientY - o.top  - ($s.height()/2);
			$s.stop(true, false).animate({ top:y, left:x }, { duration:settings.duration, easing:settings.easing, queue:false });
		}
		
		// main
		this.each(function(){
			// init
			var $c = $(this);
			var $s = $c.find(">."+settings.className);
			var $a = $c.find(">."+settings.className+">a");
			if($s.length==0) return this;
			$s.css({width:$a.find(">img").width() * $a.length});
			
			// bind
			$(this)
				.hover(
					function(e){
						if(settings.timer=="btn") return;
						settings.timer = setTimeout(function(){
							$s.show().find(">a").animate({ opacity:1 }, { duration:1000 });
							settings.timer = null;
						}, 500);
						
						$c.bind('mousemove', function(e){_stalking(e, $c, $s)});
					},
					function(){
						if(settings.timer!=null) clearTimeout(settings.timer);
						$s.find(">a").stop().animate({ opacity:0 }, { duration:500, queue:false });
					}
				)
				.find(">."+settings.className)
				.hover(
					function(){
						$c.unbind('mousemove');
						$(this).stop();
					},
					function(){
						$c.bind('mousemove', function(e){_stalking(e, $c, $s)});
					}
				)
				.end()
				.find(">.btn")
				.hover(
					function(){
						if(settings.timer!=null) clearTimeout(settings.timer);
						settings.timer = "btn";
						$a.stop().animate({ opacity:0 }, { duration:500, queue:false });
						$c.unbind('mousemove');
					},
					function(){
						settings.timer = null;
						$s.show().find(">a").animate({ opacity:1 }, { duration:1000 });
						$c.bind('mousemove', function(e){_stalking(e, $c, $s)});
					}
				);
		});
		return this;
	};
})(jQuery);

/*
 * L1 animated rollover plugin
 */
(function($){
	$.fn["animatedRollover"] = function(options){
		// settings
		var settings = $.extend({
			duration:350
		}, options);
		
		// main
		this.each(function(){
			$(this).hover(
				function(){
					$(this)
						.find(".overlay")
						.stop()
						.css({left:-690})
						.animate({left:-230}, settings.duration, "easeOutQuart");
				},
				function(){
					$(this)
						.find(".overlay")
						.stop()
						.animate({left:230}, settings.duration*0.6, "easeInCubic");
				}
			);
		});
		return this;
	};
})(jQuery);

/*
 * L1 uniqlo logo allocation plugin
 */
(function($){
	$.fn["logoAllocation"] = function(options){
		// settings
		var settings = $.extend({
			"cofMin"  : 4,
			"cofMax"  : 6,
			"retryMax":10
		}, options);
			
		var _content_logo_allocations = [];
		var _content_logo_element = $("<li />")
			.addClass("logo_block")
			.addClass("contentH10")
			.css({display:"none"});
		
		// main
		this.each(function(i, elem){
		
			// 縦4-6ブロックごとにロゴ1つ配置
			var cof = Math.floor(Math.random()*(settings.cofMax-settings.cofMin+1))+settings.cofMin;
			var len = $(this).find(">li").length;
			var cnt = Math.floor(len/cof);
			
			for(var j=0; j<cnt; j++){
				var k, h, r = settings.retryMax;
				
				while(true){
					k = cof*j+j+1;
					k += Math.floor(Math.random()*cof)+1;
					// インデックスをグリッド番号に翻訳
					h = 0;
					$(this)
						.find(">li")
						.each(function(i,elem){
							h+= ($(this).hasClass("contentH05"))?0:
								($(this).hasClass("contentH10"))?1:2;
							if(k==i+1){
								h++;
								return false;
							}
						});
					// 横並びにならないようにカブリ総当り判定
					if($.inArray(h, _content_logo_allocations)!=-1){
						$.log("かぶったー: "+i.toString()+"-"+j.toString());
						r--;
						if(r>0) continue;
					}
					break;
				}
				if(r>0){
					_content_logo_allocations.push(h);
					$(this)
						.find(">li:nth-child("+k.toString()+")")
						//.after("<li class='logo_block contentH10'></li>");
						.after(_content_logo_element.clone());
				}
			}
		});
		
		if($("#top").hasClass("selected")){
			// ie fix
			if(!$.support.opacity){
				$("#content_blocks .logo_block").show();
			}else{
				$("#content_blocks .logo_block").slideDown(600, "easeOutQuint");
			}
		}
		$.log(_content_logo_allocations);
		return this;
	};
})(jQuery);

/*
 * L1 image crossfade plugin
 */
(function($){
	$.fn["crossFade"] = function(options){
		// settings
		var settings = $.extend({
			"selector" : ">a>img:odd",
			"duration" : 400,
			// yugop demo
			//"interval" : 1330
			"interval" : 2666,
			"stop"     : false
		}, options);
		
		// timer
		if(typeof $.L1ImageCrossFadeTimer === "undefined") {
			$.extend({L1ImageCrossFadeTimer:0});
			$.L1ImageCrossFadeTimer = null;
		}
		
		// ie fix
		if(!$.support.opacity) settings.interval *= 1.5;
		
		// main
		var $target = $(this).filter(":visible").find(settings.selector);
		if(!settings.stop){
			if($.L1ImageCrossFadeTimer==null){
				$.L1ImageCrossFadeTimer = setInterval(function(){
					$target.stop(true, true).fadeToggle(settings.duration, "easeInOutQuart");
				},settings.interval);
				$.log("L1 image crossfade start: "+$.L1ImageCrossFadeTimer.toString());
			}
		}else{
			if($.L1ImageCrossFadeTimer!=null){
				clearInterval($.L1ImageCrossFadeTimer);
				$.log("L1 image crossfade stop: "+$.L1ImageCrossFadeTimer.toString());
				$.L1ImageCrossFadeTimer = null;
			}
		}
		return this;
	};
})(jQuery);

/*
 * animated page top plugin
 */
(function($){
	$.fn["animatedPageTop"] = function(options){
		// settings
		var settings = $.extend({
		}, options);
		
		// main
		this.each(function(){
			$(this).click(function(){
				//$(window).scrollTop(0);
				$("html,body").animate({scrollTop:0}, "slow", "easeOutQuart");
				$(this).blur();
				return false;
			});
		});
		return this;
	};
})(jQuery);


/*
 * following global navi
 */
(function($){
	$.fn["followGnavi"] = function(options){
		// settings
		var settings = $.extend({}, options);
		
		// main
		this.each(function(){
			$target = $(this);
			var top=$target.offset().top;
			$target.find('>div').wrap("<div id='head_wrap' style='text-align:center;width:100%'></div>");
			$target.height(settings.h);
			$(window).scroll(function(event){
				var y=$(this).scrollTop();
				if(y>top){
					$('#head_wrap').css({
						'position':'fixed',
						'top':0,
						'margin-left':10,
						'margin':'0 auto',
						'padding-top':'6px',
						'background-color': $("body").css("background-color")
					});
				}else{
					$('#head_wrap').css({
						'position':'relative',
						'top':0,
						'padding-top':'0px'
					});
				}
			});

		});
		return this;
	};
})(jQuery);

/*
 * search.do Plugin
 * @require -
 */
(function($){
	$.fn["searchQtext"] = function(options){
		var settings = $.extend({}, options);
		this.each(function(){
			$target = $(this);
			//$target.focus();

			$(document).keydown(function(e){

				setTimeout(function(){
					$("form.topSearch span.btn_reset").remove();
					if($target.val()!=""){
						$("form.topSearch").append('<span class="btn_reset">x</span>');
						$("span.btn_reset").bind('click',resetVal);
					}
				},10);
			});
		});

		function resetVal(){
			$("span.btn_reset").remove();
			$target.val("");
		}
		return this;
	};
})(jQuery);

// initialize
$(function(){
	$(".imgover").rollover();
	//$("#header").l1header().l1Banner();
	$("#header").l1header({"click":true}).l1Banner({"click":true, "thumbsToggle":false, "pauseFade":false});
	$("#content_banner .content_banner_item").stalker();
	var headerHeight = 50;
	$('#header').followGnavi({'h':headerHeight});

	// for search.do
	$("#searchFocus").searchQtext();

	// lazy evaluation
	setTimeout(function(){
		//$("#content_blocks>ul").logoAllocation();
		$("#content_blocks>ul>li").animatedRollover();
		$("#content_blocks .contentCrossFade").crossFade();
		$("#goPageTop>a").animatedPageTop();
	},600);
});

/***************
** 2013 GLOBAL NAV
** ADD 2013.12.03
***************/
;(function($){
	/***************
	** show gnav category
	** @param array option
	** @require jQuery easing plugin
	***************/
	$.fn.gnavCategoryShow = function(option) {
		var setting = {
			'navigator'    : '#navHeader',
			'sliderNav'    : '#content_banner_nav',
			'movePoint'    : '#gnav_main',
			'area'         : '.gnav_category',
			'cartArea'     : '#gnav_cart_trigger',
			'scrollClose'  : true,
			'event'        : 'click.on'
		};
		var openAnimateParam1 = {
			'param'  : {
				'opacity'  : 1
			},
			'action' : {
				'duration' : 500,
				'queue'    : false,
				'easing'   : 'easeOutQuad'
			}
		};
		var closeAnimateParam1 = {
			'param'  : {
				'opacity'  : 0.5
			},
			'action' : {
				'duration' : 500,
				'queue'    : false,
				'easing'   : 'easeInCubic',
				'complete' : function(){$(this).not('.active').css({'opasity':0}).hide();}
			}
		}
		var closeAnimateParam3 = {
			'param' : {
				'opacity'  : 0
			},
			'action' : {
				'duration' : 200,
				'queue'    : false,
				'easing'   : 'easeOutQuad',
				'complete' : function(){$(this).not('.active').hide();}
			}
		}

		$.extend(setting, option);

		var scrollAction = 'bind';
		if (setting.scrollClose == false) {
			scrollAction = 'unbind';
		}

		return $(this).each(function() {
			$(this).bind(setting.event, function(){

				var target = $(this);
				var current = target.hasClass('active');
				var showCategoryID = String(this.hash).toLowerCase().replace('!', 'gnav_');
				var clearTargetElm = $(setting.area+':not(:has('+showCategoryID+'))');

				//reset hover action
				target.parents(setting.navigator).find('>li>a.active').removeClass('active');
				target.parents(setting.navigator).find('>li:not(.gfocus):not(.focus):not(.current)>a>img').each(function(){
					$(this).attr('dsrc', $(this).attr('dsrc').replace('_o.', '.')).attr('src', $(this).attr('dsrc'));
				});

				if (!current) {
					//show under L2 contents
					$('#header').css({'overflow': 'visible'});

					//set hover action
					target.addClass('active').find('img').each(function(){
						$(this).attr('dsrc', $(this).attr('hsrc'));
						$(this).trigger('mouseover');
					});

					//close action
					$('.gnav_closeArea').css({'visibility':'hidden'});
					if ($('body').is(".ipad") || $('body').is(".androidTab")) {
						clearTargetElm.removeClass('active').css({'zIndex': 10000}).stop().css(closeAnimateParam1.param);
					} else {
						clearTargetElm.removeClass('active').css({'zIndex': 10000}).stop().animate(closeAnimateParam1.param,closeAnimateParam1.action);
					}

					//open action
					if ($('body').is(".ipad") || $('body').is(".androidTab")) {
						$(showCategoryID).parent().addClass('active').find('.gnav_closeArea').css({'visibility':'visible'}).end().css({'zIndex': 10015}).show().stop().css(openAnimateParam1.param).not('.active').hide();
					} else {
						$(showCategoryID).parent().addClass('active').find('.gnav_closeArea').css({'visibility':'visible'}).end().css({'zIndex': 10015}).show().stop().animate(openAnimateParam1.param, openAnimateParam1.action);
					}

					//hide slide arrow
					$(setting.sliderNav).css({'visibility':'hidden'});
					$(window)[scrollAction]('scroll', afterShowEvent);

					//modul window
					gnavModulWindow('show');
				} else {
					//close action
					if ($('body').is(".ipad") || $('body').is(".androidTab")) {
						$(setting.area).removeClass('active').css({'zIndex': 0}).stop().css(closeAnimateParam3.param).not('.active').hide();
					} else {
						$(setting.area).removeClass('active').css({'zIndex': 0}).stop().animate(closeAnimateParam3.param, closeAnimateParam3.action);
					}

					//show slide arrow
					$(setting.sliderNav).css({'visibility':'visible'});
					$(window).unbind('scroll', afterShowEvent);

					//modul window
					gnavModulWindow('hide');
				}

				//L2クリックでL2.5以下リセット
				resetMenu("L2");

				return false;
			});
		});
	}

	/***************
	** close category menu
	***************/
	$.fn.gnav_close = function() {
		return $(this).each(function(){
			$(this).bind('click.close', function(){
				gnavCallCloseEvent();
				return false;
			});
		});
	}
	function gnavCallCloseEvent() {
		$('#navHeader > li > a.active').triggerHandler('click');
		gnavModulWindow('hide');
	}
	
	/***************
	** change second block
	** @param array option
	***************/
	$.fn.gnavChangeSecondBlock = function(option) {
		var setting = {
			'on' : 'mouseover.mover',
			'off' : 'mouseout.mout',
			'time' : (1000/10)
		};

		$.extend(setting, option);
		var globalGnavSecondBlockAction = null;

		return $(this).each(function() {
			$(this).bind(setting.on, function(){
				var target = $(this);
				globalGnavSecondBlockAction = setTimeout(function(){
					if (target.hasClass('active')) {return false;}
					var parentBlock = target.parents('div.classBlock');
					parentBlock.find('div.gnav_firstBlock a.active').removeClass('active').find('img').trigger('mouseout');
					target.addClass('active').find('img').trigger('mouseover');
					parentBlock.find('div.gnav_secondBlock ul').removeClass('active').hide();
					$(target.get(0).hash).addClass('active').css({'opacity':1}).show();
					return false;
				}, setting.time);
			}).bind(setting.off, function(){
				clearTimeout(globalGnavSecondBlockAction);
			}).bind('click.cancel', function(){
				// 第1階層上にあるa hrefリンクをクリックした場合
				if (
				    $(this).parent('li').parent('ul').hasClass('gnav_firstDirectLinkList') &&
				    $(this).attr('href').substr(0, 1) != '#'
				) {
					gnavCallCloseEvent();
					return true;
				}
				return false;
			});
		});
	}

	/***************
	** monitoring vertical scroll Event
	***************/
	function afterShowEvent() {
		$('#navHeader > li > a.active').each(function(){
			var target = $(this);
			var clickHandle = target.attr('onclick');
			target.attr('onclick', '').triggerHandler('click');
			target.attr('onclick', clickHandle);
		});
		$(window).unbind('scroll', afterShowEvent);
		return false;
	}

	/***************
	** modul window
	***************/
	function setGnavModul(isPc) {
		var mWindow = $('<div>');
		mWindow.addClass('gnav_modul_window').css({
			'position'           : 'fixed',
			'top'                : 0,
			'left'               : 0,
			'opacity'            : 0.8,
			'width'              : '100%',
			'height'             : '100%',
			'backgroundColor'    : '#ffffff',
			'zIndex'             : '9000',
			'display'            : 'none'
		});
		if (isPc == false) {
			mWindow.gnav_close();
		}
		$('#header').after(mWindow);
	}
	function gnavModulWindow(action) {
		var target = $('.gnav_modul_window');
		target[action]();
	}

	/***************
	** clear search text
	***************/
	$.fn.clearSearchText = function(target) {
		var btn = $(this);
		var textArea = $(target);
		btn.click(function() {
			textArea.val("");
			btn.hide();
		});
		textArea.bind('keyup blur', function() {
			if (textArea.val() != "") {
				btn.show();
			} else {
				btn.hide();
			}
		});
	}

	/***************
	** Change UA Style
	***************/
	function setBodyIni(){
		//get UserAgent
		var ua = navigator.userAgent;
		var changeSecondBlockOption = {'on': 'click.mtouchstart', 'off': 'mouseup.mtouchend', 'time': 0};
		var closerTaregt = '.siteLogo a, .gnav_closeArea';
		var bodyClass = '';
		var scrollAction = {'scrollClose': true};
		var isPc = false;

		if (ua.match(/ipad/i)){
			//iPad
			bodyClass = 'ipad';
			scrollAction = {'scrollClose': false};
		} else if (ua.match(/android/i) && !ua.match(/mobile/i)){
			//Android Tab
			bodyClass = 'androidTab';
			scrollAction = {'scrollClose': false};
		} else {
			//PC
			if (ua.match(/msie [8.]/i)) {
				bodyClass = 'ie ltIE8';
			} else if (ua.match(/msie [7.]/i)) {
				bodyClass = 'ie ltIE8 ltIE7';
			} else if (ua.match(/msie/i)) {
				bodyClass = 'ie';
			} else if (ua.match(/macintosh/i)) {
				ua.match(/Mac OS X ([0-9_]+)/ig);
				var osVer = String(RegExp.$1).replace("_", ".").replace(/_/g, "");
				if (osVer < 10.7) {
					bodyClass = 'mac ltWebK2';
				} else {
					bodyClass = 'mac';
				}
			}
			changeSecondBlockOption = {};
		}
		setGnavModul(isPc);
		$('#gnav_header #navHeader > li > a').gnavCategoryShow(scrollAction);
		$('body').addClass(bodyClass);
		$('div.gnav_firstBlock ul li a').gnavChangeSecondBlock(changeSecondBlockOption);
		$(closerTaregt).gnav_close();
	};

	/***************
	** link bullets
	***************/
	$.fn.setGnavBullets = function() {
		var target = $(this);
		return target.each(function(){
			$(this).append($('<span>').addClass('iconNav'));
		});
	}

	/***************
	** menu reset
	***************/
	function resetMenu() {
		var headerScope = $('#gnav_header');
		var level = arguments.length;
		//reset nav action
		//デフォルト実行
		if (!level) {
			//L2のactiveをリセット（TOP対策のためクリックで）
			headerScope.find('#navHeader > li').find('> a.active').each(function(){
				var target = $(this);
				var clickHandle = target.attr('onclick');
				target.attr('onclick', '').triggerHandler('click');
				target.attr('onclick', clickHandle);
			});
			gnavCallCloseEvent();
		}
		//L2.5初期位置にリセット
		headerScope.find('#navHeader > li').find('div.gnav_firstBlock ul li:eq(0) a').trigger('mouseover').each(function(){
			var target = $(this);
			var clickHandle = target.attr('onclick');
			target.attr('onclick', '').triggerHandler('click');
			target.attr('onclick', clickHandle);
		});
		//L3のアクティブをリセット
		headerScope.find('#navHeader > li').find('div.gnav_secondBlock ul li a').trigger('mouseout');
		//L2カレントが初期展開されているようにする
		headerScope.find('#navHeader > li').find('div.gnav_firstBlock ul li.gfocus a').trigger('mouseover').each(function(){
			var target = $(this);
			var clickHandle = target.attr('onclick');
			target.attr('onclick', '').triggerHandler('click');
			target.attr('onclick', clickHandle);
		});
	}

	/***************
	** page init
	***************/
	function ini() {
		var headerScope = $('#gnav_header');

		if (headerScope.length) {
			//set nav action
			setBodyIni();
			headerScope.find('#navHeader > li > a img').rollover();
			headerScope.find('#navUtil li:not(#gnav_search) a img').rollover();
			headerScope.find('#navUtil.gnav_util_slider li:not(#gnav_search_slide) a img').rollover();
			headerScope.find('.gnav_category div.classBlock div.gnav_firstBlock ul li a img').rollover();
			headerScope.find('#gnav_textClear').clearSearchText('#searchFocus');
			headerScope.find('div.gnav_thirdBlock a.iconNav').setGnavBullets();
			headerScope.find('div.classBlock > div').not('.gnav_firstBlock').not('.gnav_closeArea').add('#navUtil li').find('a').bind('click', function(e) {
				resetMenu();
			});
			headerScope.find('div.classBlock > div').not('.gnav_firstBlock').not('.gnav_closeArea').add('#navUtil.gnav_util_slider li').find('a').bind('click', function(e) {
				resetMenu();
			});
			$("#gnav_header a, #gnav_header area").unbind("click.anc");
			resetMenu();
		}else{
			$("#header").l1header().l1Banner();
		}
	}

    /***************
	** top roll over
	***************/
	$(function () { $(".content_banner_item .btn img").rollover(); });

	$(document).ready(function(){ini();});
})(jQuery);